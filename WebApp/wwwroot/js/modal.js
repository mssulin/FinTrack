document.addEventListener("DOMContentLoaded", () => {
    // 🔹 öppna modal
    document.querySelectorAll("[data-modal-target]").forEach(btn => {
        btn.addEventListener("click", () => {
            const modalId = btn.getAttribute("data-modal-target");
            const modal = document.getElementById(modalId);
            if (modal) {
                modal.style.display = "flex";

                // 🔹 extra för historik-modals
                if (modalId.startsWith("history-modal-")) {
                    const savingId = modalId.split("-").pop();
                    const contentEl = document.getElementById(`history-content-${savingId}`);

                    if (contentEl) {
                        contentEl.innerHTML = "<p>Laddar historik...</p>";

                        fetch(`/Savings/GetHistory?savingId=${savingId}`)
                            .then(res => res.text())
                            .then(html => {
                                contentEl.innerHTML = html;
                            })
                            .catch(() => {
                                contentEl.innerHTML = "<p>Kunde inte ladda historik.</p>";
                            });
                    }
                }
            }
        });
    });

    // 🔹 stäng modal (x-knappen)
    document.querySelectorAll(".modal .close").forEach(closeBtn => {
        closeBtn.addEventListener("click", () => {
            closeBtn.closest(".modal").style.display = "none";
        });
    });

    // 🔹 stäng modal om man klickar på overlay
    window.addEventListener("click", (event) => {
        if (event.target.classList.contains("modal")) {
            event.target.style.display = "none";
        }
    });

    // 🔹 IMAGE PREVIEW
    const fileInput = document.getElementById("image");
    const previewImg = document.getElementById("subImagePreview");
    const iconContainer = document.querySelector(".image-preview-icon-container");
    const previewContainer = document.querySelector(".image-preview-container");

    // Gör preview-rutan klickbar för att öppna filväljaren
    if (previewContainer && fileInput) {
        previewContainer.addEventListener("click", () => {
            fileInput.click();
        });
    }

    // Visa preview när en bild väljs
    if (fileInput) {
        fileInput.addEventListener("change", (e) => {
            const file = e.target.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = (ev) => {
                    previewImg.src = ev.target.result;
                    previewImg.classList.remove("hide");
                    if (iconContainer) iconContainer.classList.add("hide");
                };
                reader.readAsDataURL(file);
            }
        });
    }
});