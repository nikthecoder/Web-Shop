// Send cart to server and get a list of products back
function LoadCart() {
    var JsonLocalStorageObj = JSON.stringify(localStorage);

    // Vet inte riktigt varför det funkar, men det funkar :D
    // Fick slåss mot en massa error 400, 405, och 415 :)
    $.ajax({
        url: "/GetCartItems",
        type: "POST",
        data: JsonLocalStorageObj,
        contentType: "application/json; charset=utf-8",
        success: function (result) {
            createCardWithItems(result);
        },
        error: function (xhr, status, error) {
            console.log('Error : ' + xhr.responseText);
        }
    });
}

// Wait for the page to load, then update the list
window.addEventListener("load", () => {
    LoadCart();
});

// Convert Json to html
function createCardWithItems(jsonData)
{
    var json = JSON.parse(jsonData);
    var container = document.querySelector("#allItems");
    removeAllChildren(container);

    if (json.length === 0) {
        let empty = document.createElement("h3");
        empty.textContent = "Din varukorg är tom!";
        empty.className = "text-center";
        container.appendChild(empty);
    }
    else {
        for (var i = 0; i < json.length; i++) {

            var template = document.querySelector("template");
            var holder = template.content.firstElementChild.cloneNode(true);

            var img = holder.querySelector("#imageURL");
            img.src = json[i].Item.ImageURL;
            img.alt = json[i].Item.Product;

            var product = holder.querySelector("#product");
            product.textContent = json[i].Item.Product;
            var description = holder.querySelector("#description");
            description.textContent = json[i].Item.Description;

            var quantity = holder.querySelector(".form1");
            quantity.value = json[i].Amount;
            quantity.max = json[i].Item.Stock;
            quantity.classList.add(json[i].Item.ID);
            holder.querySelector(".minus").name = json[i].Item.ID;;
            holder.querySelector(".plus").name = json[i].Item.ID;;

            var unitPrice = holder.querySelector(".unitPrice");
            unitPrice.textContent = parseFloat(json[i].Item.Price).toFixed(2) + " kr/st";
            var totalUnitPrice = holder.querySelector(".totalUnitPrice");
            totalUnitPrice.textContent = parseFloat((json[i].Amount) * parseFloat(json[i].Item.Price)).toFixed(2);
            
            var removeBtn = holder.querySelector(".remove-from-cart");
            removeBtn.name = json[i].Item.ID;

            container.appendChild(holder);
        }
    }

    var allRemoveBtns = container.getElementsByClassName('remove-from-cart');
    function removeButtons() {
        for (const removeBtn of allRemoveBtns) {
            removeBtn.onclick = event => {
                let productClicked = removeBtn.name;
                shoppingCart.delete(productClicked);
                writeLocalStorage();
                LoadCart();
                numberOfItemsInCart();
            }
        };
    }
    removeButtons();

    var allMinusBtn = container.getElementsByClassName('minus');
    function minusButtons() {
        for (const minus of allMinusBtn) {
            minus.onclick = event => {
                var input = container.getElementsByClassName(minus.name)[0];

                if (input.value > 1) {
                    input.value -= 1;
                }
                shoppingCart.set((minus.name), parseInt(input.value));
                writeLocalStorage();
                LoadCart();
                numberOfItemsInCart();
            }
        };
    }
    minusButtons();

    var allPlusBtn = container.getElementsByClassName('plus');
    function plusButtons() {
        for (const plus of allPlusBtn) {
            plus.onclick = event => {
                var input = container.getElementsByClassName(plus.name)[0];
                var current = parseInt(input.value);
                if (current < input.max) {
                    input.value = current + 1;
                }

                shoppingCart.set((plus.name), parseInt(input.value));
                writeLocalStorage();
                LoadCart();
                numberOfItemsInCart();
            }
        };
    }
    plusButtons();


    setValue("totalAmount", GetTotalAmount());
    if (GetTotalAmount() === 0) {
        document.querySelector(".bi-cart").style.color = null;
        document.querySelector("#amount").textContent = "";
        document.querySelector(".btnBuy").classList.add("disabled");
    }

    setValue("totalPrice", GetTotalPrice());
    setValue("totalPrice2", GetTotalPrice());
}
// check if element exists first
function setValue(id, value) {
    var element = document.getElementById(id);
    if (element) {
        element.innerHTML = value;
    }
}

function removeAllChildren(parent) {
    while (parent.firstChild) {
        parent.removeChild(parent.firstChild);
    }
}

function GetTotalAmount() {
    let container = document.querySelector("#allItems");
    let inputs = container.querySelectorAll("input");
    let total = 0;
    for (const input of inputs) {
        total += parseInt(input.value);
    };
    return total;
}

function GetTotalPrice() {
    let container = document.querySelector("#allItems");
    let inputs = container.querySelectorAll(".totalUnitPrice");

    let total = 0;
    for (const input of inputs) {
        total += parseFloat(input.textContent);
    };

    total = parseFloat(total).toFixed(2);

    return total;
}

function EmptyCart() {
    var empty = confirm("Är du säker på att du vill tömma din varukorg?");
    if (empty) {
        localStorage.clear();
        LoadCart();
    }
}

// these are repeats from site.js :((
function writeLocalStorage() {
    localStorage.setItem('shopping-cart', JSON.stringify(Object.fromEntries(shoppingCart)));
}
function readLocalStorage() {
    let cartStorage = JSON.parse(localStorage.getItem('shopping-cart'));

    if (cartStorage != null) {
        return new Map(Object.entries(cartStorage));
    };
}
function numberOfItemsInCart() {
    let total = 0;

    var mapIter = shoppingCart.values();
    shoppingCart.forEach(value => {
        total += mapIter.next().value;
    });
    let number = document.getElementById("amount");
    if (total !== 0) {
        number.textContent = total + " ";
        amount.style.color = "green";
    }

    if (shoppingCart.size > 0) {
        shoppingCartElt.style.color = "green";
    }
}