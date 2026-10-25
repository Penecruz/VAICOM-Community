(function () {
  const DEFAULT_ENDPOINT = "./mock-api/dashboard.json";
  const params = new URLSearchParams(window.location.search);
  const endpoint = params.get("endpoint") || DEFAULT_ENDPOINT;

  const endpointLabel = document.getElementById("endpointLabel");
  const summaryGrid = document.getElementById("summaryGrid");
  const commsList = document.getElementById("commsList");
  const markersList = document.getElementById("markersList");

  endpointLabel.textContent = `Data source: ${endpoint}`;

  function renderSummary(data) {
    const cards = [
      ["Module", data.module],
      ["Callsign", data.callsign],
      ["Theater", data.theater],
      ["Mission", data.mission],
      ["Status", data.status],
      ["Last Update", data.lastUpdateUtc]
    ];

    summaryGrid.innerHTML = cards
      .map(([label, value]) => `<article class=\"card\"><div class=\"label\">${label}</div><div class=\"value\">${value || "-"}</div></article>`)
      .join("");
  }

  function renderComms(comms) {
    commsList.innerHTML = (comms || [])
      .map(item => `<li><strong>${item.time}</strong> — ${item.tx}: ${item.message}</li>`)
      .join("");
  }

  function renderMarkers(markers) {
    markersList.innerHTML = (markers || [])
      .map(item => `<li>${item.text} (${item.pos.x.toFixed(1)}, ${item.pos.z.toFixed(1)})</li>`)
      .join("");
  }

  async function loadDashboard() {
    try {
      const response = await fetch(endpoint, { cache: "no-store" });
      if (!response.ok) {
        throw new Error(`HTTP ${response.status}`);
      }

      const data = await response.json();
      renderSummary(data);
      renderComms(data.recentComms);
      renderMarkers(data.markers);
    } catch (error) {
      summaryGrid.innerHTML = `<article class=\"card\"><div class=\"label\">Error</div><div class=\"value\">${error.message}</div></article>`;
      commsList.innerHTML = "";
      markersList.innerHTML = "";
    }
  }

  loadDashboard();
  setInterval(loadDashboard, 5000);
})();
