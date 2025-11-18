(function () {
    const overlay = document.getElementById("searchOverlay");
    const popup = document.getElementById("searchPopup");
    const searchInput = document.getElementById("searchInput");
    const suggestList = document.getElementById("suggestList");
    const searchIcon = document.querySelector(".search-icon img");

    // ----- Hàm tiện ích -----
    function openPopup() {
        overlay.style.display = "block";
        popup.style.display = "block";
        popup.classList.add("show");
        searchInput.focus();
    }

    function closePopup() {
        overlay.style.display = "none";
        popup.classList.remove("show");
        popup.style.display = "none";
        suggestList.innerHTML = "";
        suggestList.style.display = "none";
    }

    function renderSuggestions(list) {
        suggestList.innerHTML = "";
        if (!list || list.length === 0) {
            suggestList.style.display = "none";
            return;
        }

        suggestList.style.display = "block";
        list.forEach(item => {
            const li = document.createElement("li");
            li.className = "suggest-item";

            // ảnh
            if (item.Image) {
                const img = document.createElement("img");
                img.src = item.Image.startsWith("/")
                    ? item.Image
                    : "/Content/images/" + item.Image;
                img.className = "suggest-img";
                li.appendChild(img);
            }

            // tên
            const text = document.createElement("span");
            text.textContent = item.ProductName;
            li.appendChild(text);

            // click → sang trang Details
            li.addEventListener("click", () => {
                window.location.href = "/Produce/Details/" + item.ProductID;
            });

            suggestList.appendChild(li);
        });
    }

    // ----- Event -----
    if (searchIcon) searchIcon.addEventListener("click", openPopup);
    overlay.addEventListener("click", closePopup);

    searchInput.addEventListener("input", () => {
        const kw = searchInput.value.trim();
        if (!kw) {
            suggestList.innerHTML = "";
            suggestList.style.display = "none";
            return;
        }

        fetch("/Produce/GetSuggest?keyword=" + encodeURIComponent(kw))
            .then(res => res.json())
            .then(data => renderSuggestions(data))
            .catch(err => console.error("Suggest Error:", err));
    });

    searchInput.addEventListener("keydown", e => {
        if (e.key === "Enter") {
            e.preventDefault();
            window.location.href = "/Produce/Search?keyword=" + encodeURIComponent(searchInput.value);
        }
    });
})();
