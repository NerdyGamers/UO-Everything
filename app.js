/* UO-Everything wiki app: hash-routed SPA, markdown rendered client-side. */
(function () {
  "use strict";
  var RAW = null;
  var SITE = null;
  var sectionCache = {};
  var searchIdx = null;
  var searchLoading = null;

  var contentEl = document.getElementById("content");
  var crumbsEl = document.getElementById("crumbs");
  var sidebarEl = document.getElementById("sidebar");
  var searchEl = document.getElementById("search");
  var resultsEl = document.getElementById("results");

  function esc(s) {
    return String(s).replace(/[&<>"]/g, function (c) {
      return { "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;" }[c];
    });
  }

  function normalize(p) {
    var parts = p.split("/");
    var out = [];
    for (var i = 0; i < parts.length; i++) {
      var x = parts[i];
      if (x === "" || x === ".") continue;
      if (x === "..") { out.pop(); continue; }
      out.push(x);
    }
    return out.join("/");
  }

  function fetchText(url) {
    return fetch(url).then(function (r) {
      if (!r.ok) return { ok: false };
      return r.text().then(function (t) { return { ok: true, text: t }; });
    }).catch(function () { return { ok: false }; });
  }

  function routePath() {
    var h = location.hash || "";
    if (h.slice(0, 2) === "#/") h = h.slice(2);
    else if (h.charAt(0) === "#") h = h.slice(1);
    h = decodeURIComponent(h).replace(/\/+$/, "");
    return h;
  }

  function pageTitle(md) {
    var m = md.match(/^#\s+(.+)$/m);
    if (!m) return null;
    return m[1].replace(/\*\*(.+?)\*\*/g, "$1").replace(/^\*+|\*+$/g, "").trim();
  }

  function setCrumbs(items) {
    // items: [[label, route|null]]
    crumbsEl.innerHTML = items.map(function (it, i) {
      var sep = i > 0 ? " &rsaquo; " : "";
      if (it[1] == null) return sep + "<span>" + esc(it[0]) + "</span>";
      return sep + '<a href="#/' + esc(it[1]) + '">' + esc(it[0]) + "</a>";
    }).join("");
  }

  function crumbTrail(route, leafTitle) {
    var items = [["Home", ""]];
    if (!route) { setCrumbs([["Home", null]]); return; }
    var parts = route.split("/");
    var acc = "";
    for (var i = 0; i < parts.length; i++) {
      acc = acc ? acc + "/" + parts[i] : parts[i];
      var label = i === parts.length - 1 && leafTitle ? leafTitle
        : parts[i].replace(/-/g, " ").replace(/\b\w/g, function (c) { return c.toUpperCase(); });
      items.push(i === parts.length - 1 ? [label, null] : [label, acc]);
    }
    setCrumbs(items);
  }

  function rewriteLinks(currentPath) {
    var dir = currentPath.split("/").slice(0, -1).join("/");
    function resolve(href) { return normalize((dir ? dir + "/" : "") + href); }
    contentEl.querySelectorAll("a[href]").forEach(function (a) {
      var href = a.getAttribute("href");
      if (!href) return;
      if (href.charAt(0) === "#") return;
      if (/^[a-z][a-z0-9+.-]*:/i.test(href)) { a.target = "_blank"; a.rel = "noopener"; return; }
      var r = resolve(href);
      if (/\/README\.md$/i.test(r) || /^README\.md$/i.test(r)) {
        a.href = "#/" + r.replace(/\/README\.md$/i, "");
      } else if (/\.md$/i.test(r)) {
        a.href = "#/" + r.replace(/\.md$/i, "");
      } else {
        a.href = "#/" + r;
      }
    });
    contentEl.querySelectorAll("img[src]").forEach(function (img) {
      var src = img.getAttribute("src");
      if (!src || /^[a-z][a-z0-9+.-]*:/i.test(src) || src.charAt(0) === "#") return;
      img.src = RAW + "/" + resolve(src);
      img.loading = "lazy";
    });
  }

  function showPage(route, md) {
    var html = marked.parse(md);
    contentEl.innerHTML = html;
    rewriteLinks(route);
    var t = pageTitle(md) || route;
    document.title = t + " — UO-Everything Wiki";
    crumbTrail(route, t);
    window.scrollTo(0, 0);
  }

  function dirNode(section, dirpath) {
    var d = sectionCache[section];
    if (!d || !d.data) return null;
    return d.data.tree[dirpath] || null;
  }

  function showDir(route, md, section) {
    var html = marked.parse(md);
    var rel = route.split("/").slice(1).join("/");
    var node = dirNode(section, rel);
    var list = "";
    if (node && (node.dirs.length || node.files.length)) {
      list += '<div class="dirlist">';
      if (node.dirs.length) {
        list += "<ul>";
        node.dirs.forEach(function (sd) {
          var name = sd.split("/").pop().replace(/-/g, " ");
          list += '<li class="dirname"><a href="#/' + esc(route + "/" + sd.split("/").pop()) + '">' + esc(name) + "</a></li>";
        });
        list += "</ul>";
      }
      var files = node.files.filter(function (f) { return f.n.toLowerCase() !== "readme.md"; });
      if (files.length) {
        list += "<ul>";
        files.forEach(function (f) {
          list += '<li><a href="#/' + esc(route + "/" + f.n.replace(/\.md$/i, "")) + '">' + esc(f.t) + "</a></li>";
        });
        list += "</ul>";
      }
      list += "</div>";
    }
    // catalog special node under items
    if (section === "items" && rel === "") {
      var cat = sectionCache.items.data.catalog;
      if (cat) {
        list += '<div class="catalogbox"><strong>Item catalog</strong> — ' +
          cat.count.toLocaleString() + ' data pages, one per item ID. ' +
          '<a href="#/items/catalog">Browse the catalog</a></div>';
      }
    }
    contentEl.innerHTML = html + list;
    rewriteLinks(route + "/README.md");
    var t = pageTitle(md) || section;
    document.title = t + " — UO-Everything Wiki";
    crumbTrail(route, t);
    window.scrollTo(0, 0);
  }

  function showCatalog() {
    var d = sectionCache.items;
    var count = d && d.data.catalog ? d.data.catalog.count.toLocaleString() : "31,411";
    contentEl.innerHTML =
      "<h1>Item Catalog</h1>" +
      "<p>" + count + " data pages — one per named item ID, parsed from the client tiledata, with decoded tile flags, weight, height, and tile fields.</p>" +
      '<div class="catalogbox"><label><strong>Look up an item ID</strong><br>' +
      '<input id="catid" placeholder="0x0a25" spellcheck="false"></label> ' +
      '<button id="catgo">Go</button>' +
      '<p style="font-size:.85rem;color:var(--ink-soft)">Enter a hex item ID, e.g. <code>0x0a25</code> (the lantern).</p></div>';
    document.getElementById("catgo").onclick = function () {
      var v = document.getElementById("catid").value.trim().toLowerCase().replace(/^0x/, "");
      if (!/^[0-9a-f]{1,4}$/.test(v)) return;
      v = ("0000" + v).slice(-4);
      location.hash = "#/items/catalog/0x" + v;
    };
    document.getElementById("catid").addEventListener("keydown", function (e) {
      if (e.key === "Enter") document.getElementById("catgo").click();
    });
    document.title = "Item Catalog — UO-Everything Wiki";
    crumbTrail("items/catalog", "Item Catalog");
    window.scrollTo(0, 0);
  }

  function showHome() {
    fetchText(RAW + "/README.md").then(function (r) {
      var html = r.ok ? marked.parse(r.text) : "<h1>UO-Everything</h1>";
      var cards = '<div class="cards">' + SITE.sections.map(function (s) {
        return '<a class="card" href="#/' + esc(s.id) + '"><h3>' + esc(s.title) + "</h3>" +
          "<p>" + esc(s.desc || "") + "</p>" +
          '<span class="n">' + s.count + " pages</span></a>";
      }).join("") + "</div>";
      contentEl.innerHTML = html + "<h2>Browse by section</h2>" + cards;
      rewriteLinks("README.md");
      document.title = "UO-Everything Wiki";
      setCrumbs([["Home", null]]);
      window.scrollTo(0, 0);
    });
  }

  function show404(route) {
    contentEl.innerHTML = '<h1>Not found</h1><p class="error">No wiki page at <code>' +
      esc(route) + "</code>.</p>";
    document.title = "Not found — UO-Everything Wiki";
    crumbTrail(route, "Not found");
  }

  function getSection(id) {
    if (sectionCache[id]) return Promise.resolve(sectionCache[id]);
    return fetch("data/" + id + ".json").then(function (r) { return r.json(); }).then(function (data) {
      sectionCache[id] = { data: data };
      return sectionCache[id];
    });
  }

  function route() {
    var r = routePath();
    contentEl.innerHTML = '<p class="loading">Loading…</p>';
    if (!r) { showHome(); return; }
    var section = r.split("/")[0];
    var known = SITE.sections.some(function (s) { return s.id === section; });
    if (!known) { show404(r); return; }
    getSection(section).then(function () {
      renderSidebar(section, r);
      if (r === "items/catalog") { showCatalog(); return; }
      fetchText(RAW + "/" + r + ".md").then(function (pg) {
        if (pg.ok) { showPage(r, pg.text); return; }
        fetchText(RAW + "/" + r + "/README.md").then(function (dr) {
          if (dr.ok) showDir(r, dr.text, section);
          else show404(r);
        });
      });
    });
  }

  function treeHtml(sec, node, base, cur) {
    var h = "<ul>";
    node.dirs.forEach(function (d) {
      var sub = sec.data.tree[d];
      if (!sub) return;
      var name = d.split("/").pop().replace(/-/g, " ");
      var open = cur === sec.id + "/" + d || cur.indexOf(sec.id + "/" + d + "/") === 0 ? " open" : "";
      h += "<li><details" + open + "><summary>" + esc(name) + "</summary>" +
        treeHtml(sec, sub, d, cur) + "</details></li>";
    });
    node.files.forEach(function (f) {
      var p = sec.id + "/" + (base ? base + "/" : "") + f.n.replace(/\.md$/i, "");
      var cls = p === cur ? ' class="cur"' : "";
      h += '<li><a href="#/' + p + '"' + cls + ">" + esc(f.t) + "</a></li>";
    });
    return h + "</ul>";
  }

  function renderSidebar(section, cur) {
    var html = "<h3>Sections</h3><ul>" + SITE.sections.map(function (s) {
      var cls = section === s.id ? ' class="active"' : "";
      return '<li><a href="#/' + esc(s.id) + '"' + cls + ">" + esc(s.title) +
        ' <span class="count">' + s.count + "</span></a></li>";
    }).join("") + "</ul>";
    if (section && sectionCache[section]) {
      var sec = { id: section, data: sectionCache[section].data };
      html += '<h3 class="treetitle">In this section</h3><div class="sectree">' +
        treeHtml(sec, sec.data.tree[""], "", cur || "") + "</div>";
    }
    html += '<h3>Reference</h3><ul><li><a href="#/items/catalog">Item catalog lookup</a></li></ul>';
    sidebarEl.innerHTML = html;
  }

  function ensureSearchIndex() {
    if (searchIdx) return Promise.resolve();
    if (searchLoading) return searchLoading;
    searchLoading = Promise.all(SITE.sections.map(function (s) { return getSection(s.id); }))
      .then(function () {
        searchIdx = [];
        SITE.sections.forEach(function (s) {
          var d = sectionCache[s.id].data;
          d.index.forEach(function (e) { searchIdx.push({ p: e.p, t: e.t, s: s.title }); });
        });
      });
    return searchLoading;
  }

  function onSearch() {
    var q = searchEl.value.trim().toLowerCase();
    if (q.length < 2) { resultsEl.hidden = true; return; }
    ensureSearchIndex().then(function () {
      var hits = [];
      for (var i = 0; i < searchIdx.length && hits.length < 30; i++) {
        if (searchIdx[i].t.toLowerCase().indexOf(q) !== -1) hits.push(searchIdx[i]);
      }
      if (!hits.length) {
        resultsEl.innerHTML = '<a><span>No matches</span></a>';
      } else {
        resultsEl.innerHTML = hits.map(function (h) {
          return '<a href="#/' + esc(h.p.replace(/\.md$/i, "")) + '"><span>' + esc(h.t) +
            '</span><span class="rpath">' + esc(h.s + " · " + h.p) + "</span></a>";
        }).join("");
      }
      resultsEl.hidden = false;
    });
  }

  function init() {
    fetch("site.json").then(function (r) { return r.json(); }).then(function (site) {
      SITE = site;
      RAW = site.raw;
      renderSidebar();
      window.addEventListener("hashchange", route);
      searchEl.addEventListener("input", onSearch);
      searchEl.addEventListener("focus", function () { ensureSearchIndex(); });
      searchEl.addEventListener("blur", function () {
        setTimeout(function () { resultsEl.hidden = true; }, 200);
      });
      document.addEventListener("click", function (e) {
        if (e.target.closest && e.target.closest("#results a")) resultsEl.hidden = true;
      });
      route();
    }).catch(function () {
      contentEl.innerHTML = '<p class="error">Could not load the wiki index.</p>';
    });
  }

  init();
})();
