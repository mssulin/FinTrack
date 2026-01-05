document.addEventListener("DOMContentLoaded", () => {
    const canvas = document.getElementById("expenseChart");
    if (!canvas) return;

    const ctx = canvas.getContext("2d");

 
    const categories = JSON.parse(canvas.getAttribute("data-categories") || "[]");
    const amounts = JSON.parse(canvas.getAttribute("data-amounts") || "[]");
    
    const baseColors = {
        "Boende": ["#38bdf8", "#0ea5e9"],    // sky
        "Mat": ["#f87171", "#dc2626"],       // rose/red
        "Transport": ["#4ade80", "#16a34a"], // emerald
        "Shopping": ["#c084fc", "#7e22ce"],  // violet/purple
        "Nöje": ["#fbbf24", "#d97706"],      // amber
        "Husdjur": ["#fef08a", "#eab308"],   // yellow
    };

   
    const backgroundGradients = categories.map((cat, i) => {
        const [light, dark] = baseColors[cat] || ["#94a3b8", "#475569"]; // fallback (grå)
        const gradient = ctx.createLinearGradient(0, 0, 0, canvas.height);
        gradient.addColorStop(0, light);
        gradient.addColorStop(1, dark);
        return gradient;
    });

    new Chart(ctx, {
        type: "bar",
        data: {
            labels: categories,
            datasets: [{
                label: "Utgifter (kr)",
                data: amounts,
                backgroundColor: backgroundGradients,
                borderRadius: 8,
                borderSkipped: false,
                maxBarThickness: 42,
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            layout: { padding: 4 },
            plugins: {
                legend: { display: false },
                tooltip: {
                    callbacks: {
                        label: (ctx) => {
                            const v = ctx.parsed.y ?? 0;
                            return new Intl.NumberFormat("sv-SE", {
                                style: "currency",
                                currency: "SEK",
                                maximumFractionDigits: 0
                            }).format(v);
                        }
                    }
                }
            },
            scales: {
                x: {
                    grid: { color: "rgba(255,255,255,0.06)" },
                    ticks: { color: "#CBD5E1" }
                },
                y: {
                    beginAtZero: true,
                    grid: { color: "rgba(255,255,255,0.06)" },
                    ticks: { color: "#CBD5E1" }
                }
            }
        }
    });
});