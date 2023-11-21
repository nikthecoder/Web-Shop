function LoadCart() {
    var JsonLocalStorageObj = JSON.stringify(localStorage);
    $.ajax({
        url: "/GetCartItems",
        type: "POST",
        data: JsonLocalStorageObj,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            var json = JSON.parse(result);
            if (json.length === 0) {
                document.querySelector(".confirmBtn").classList.add("disabled");
            }
            else {
                document.querySelector(".confirmBtn").classList.remove("disabled");
            }
            GetTotalSum(result);
        },
        error: function (xhr, status, error) {
            console.log('Error : ' + xhr.responseText);
        }
    });
}
window.addEventListener("load", () => {
    LoadCart();
});

function GetTotalSum(jsonData) {
    var json = JSON.parse(jsonData);
    var container = document.querySelector("#totalSum");
    var total = 0;

    for (var i = 0; i < json.length; i++) {
        total += (json[i].Amount) * (json[i].Item.Price)
    }
    container.textContent = parseFloat(total).toFixed(2) + " kr";
}

function PlaceOrder() {
    var JsonLocalStorageObj = JSON.stringify(localStorage);
    $.ajax({
        url: "/GenerateOrder",
        type: "POST",
        data: JsonLocalStorageObj,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            var newPath = "/Invoice/" + result;
            window.location.pathname = newPath;
        },
        error: function (xhr, status, error) {
            console.log('Error : ' + xhr.responseText);
        }
    });

    localStorage.clear();
    document.querySelector(".bi-cart").style.color = null;
    document.querySelector("#amount").textContent = "";
}