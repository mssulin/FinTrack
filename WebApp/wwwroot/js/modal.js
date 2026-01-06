document.addEventListener("DOMContentLoaded", () => {

    // Modal dropdown
    const closeAllDropdowns = () => {
        document.querySelectorAll("[data-dd-root]").forEach(root => {
            root.querySelector("[data-dd-menu]")?.classList.add("hidden");
            root.querySelector("[data-dd-button]")?.setAttribute("aria-expanded", "false");
        });
    };

    document.addEventListener("click", (e) => {
        const root = e.target.closest("[data-dd-root]");
        
        if (!root) {
            closeAllDropdowns();
            return;
        }

        // Klick inne i dropdown
        e.stopPropagation();

        const btn = root.querySelector("[data-dd-button]");
        const menu = root.querySelector("[data-dd-menu]");
        const label = root.querySelector("[data-dd-label]");
        const hidden = root.querySelector("[data-dd-hidden]");
        if (!btn || !menu || !label || !hidden) return;

        // Toggle
        if (e.target.closest("[data-dd-button]")) {
            const isOpen = !menu.classList.contains("hidden");
            closeAllDropdowns();
            if (!isOpen) {
                menu.classList.remove("hidden");
                btn.setAttribute("aria-expanded", "true");
            }
            return;
        }

        // Items
        const item = e.target.closest("[data-dd-item]");
        if (item) {
            const value = item.getAttribute("data-dd-item") ?? "";
            const text  = item.getAttribute("data-dd-text") ?? value;

            label.textContent = text;
            hidden.value = value;

            menu.classList.add("hidden");
            btn.setAttribute("aria-expanded", "false");
        }
    });

    // Öppna/stäng modal
    const openModal = async (modalId) => {
        const modal = document.getElementById(modalId);
        if (!modal) return;

        modal.classList.add("is-open");

        // Historik
        if (modalId.startsWith("history-modal-")) {
            const savingId = modalId.split("-").pop();
            const contentEl = document.getElementById(`history-content-${savingId}`);
            if (!contentEl) return;

            contentEl.innerHTML = "<p>Laddar historik...</p>";

            try {
                const res = await fetch(`/Savings/GetHistory?savingId=${savingId}`);
                contentEl.innerHTML = await res.text();
            } catch {
                contentEl.innerHTML = "<p>Kunde inte ladda historik.</p>";
            }
        }
    };

    const closeModal = (modal) => {
        if (!modal) return;
        modal.classList.remove("is-open");
        closeAllDropdowns(); 
    };

    // Öppna modal
    document.querySelectorAll("[data-modal-target]").forEach(btn => {
        btn.addEventListener("click", () => {
            openModal(btn.getAttribute("data-modal-target"));
        });
    });

    // Stäng via knapp
    document.querySelectorAll(".modal .modal-close, .modal .close").forEach(closeBtn => {
        closeBtn.addEventListener("click", () => {
            closeModal(closeBtn.closest(".modal"));
        });
    });

    // Stäng via overlay-klick
    document.addEventListener("click", (e) => {
        if (e.target.classList.contains("modal")) {
            closeModal(e.target);
        }
    });

    // Stäng modaler och dropdowns med ESC
    document.addEventListener("keydown", (e) => {
        if (e.key !== "Escape") return;

        // stäng dropdowns
        closeAllDropdowns();

        // stäng öppna modaler
        document.querySelectorAll(".modal.is-open").forEach(closeModal);
    });

    // Image preview
    const fileInput = document.getElementById("image");
    const imageBox = document.getElementById("imageBox");
    const previewImg = document.getElementById("subImagePreview");
    const placeholder = document.getElementById("imagePlaceholder");
    
    if (imageBox && fileInput) {
        imageBox.addEventListener("click", () => fileInput.click());
    }

    if (fileInput) {
        fileInput.addEventListener("change", (e) => {
            const file = e.target.files?.[0];
            if (!file) return;

            const reader = new FileReader();
            reader.onload = (ev) => {
                previewImg.src = ev.target.result;
                previewImg.classList.remove("hidden");
                placeholder?.classList.add("hidden");
            };
            reader.readAsDataURL(file);
        });
    }
});