//for testing purposes only
//overrides the alert so that there isn't any popping up when testing
function alert(){}


function addToCart(flavor, quantity) {
    //var flavor = this.flavor;
    //document.getElementById("chooseAFlavor").value;
    //var quantity = this.quantity;
        //document.getElementById("quantityOfCookie").value;
    //var returnData; 

    //if (flavor === "Choose a Flavor") {
    //    alert("Please select a flavor");
    //    return;
    //} else
    if (quantity === null || quantity <= 0) {
        alert("Please Select a valid quantity");
        return "Not accurate quantity";
    } 

    var userID = get_cookie('userID');

    var http = new XMLHttpRequest();
    http.onreadystatechange = function(){
        if (http.readyState === 4) {
            alert(http.responseText);
            document.getElementById('cartItemDisplay').innerHTML = http.responseText;   
        }
    };

    http.open("GET", "http://localhost:8742/api/values/AddToCart/?flavor=" + flavor + "&quantity=" + quantity + "&userID=" + userID, true); // true for asynchronous 
    http.send(null);
}

function showInventory() {
    var http = new XMLHttpRequest();
    http.onreadystatechange = function () {
        if (http.readyState === 4) {
            document.getElementById('currentInventory').innerHTML = http.responseText;

        }
    };

    http.open("GET", "http://localhost:8742/api/values/inventoryToString", true); // true for asynchronous 
    http.send(null);
}

//inventory in this function refers to the contents in the users cart.
function updateCart() {   
    var http = new XMLHttpRequest();
    http.onreadystatechange = function () {
        if (http.readyState === 4) {
            var twoStrings = (http.responseText).split('|');

            //checking to see if the cart is empty or if things aren't initialized how they should be
            if (twoStrings[0] == "no dictionary" || twoStrings[0] == "User hasn't been initialized in the user dictionary -- user must go to the cookie cache page/add something to the cart") {
                document.getElementById('cartTable').innerHTML = "Your Cart is currently empty --let's change that!";
                return;
            } else if (twoStrings[0] == "Your Cart is empty! (No user set)" || twoStrings[0] == "Cart is empty" || twoStrings[0] == "Your cart list is empty") {
                document.getElementById('cartTable').innerHTML = "Your Cart is currently empty --let's change that!";
                return;
            }

            var inventory = (twoStrings[0]).split(',');
            if (get_cookie('userID') == undefined || get_cookie('userID') == '') {
                document.getElementById('cartTable').innerHTML = "Your Cart is currently empty --let's change that!";
                return;
            }


            var price = (twoStrings[1]).split(',');
            var maxflav = parseInt(inventory[0]);
            var flavString;
            var quanString;
            var priceString;
            var totalString;
            var totalPricePerItem;
            var totalPrice = 0;
            var priceIndex = 1;
            var table = document.getElementById('cartTable');

            //while (table.rows.length > 2) {
            //    table.deleteRow(2)
            //}

            //var row;
            //var flavorCell;
            //var pricePerCookieCell;
            //var quantityCell;
            //var totalPriceCell;
            //var removeCell;

            //for (i = 1; i < inventory.length; i = i + 2) {
            //    row = table.insertRow();
            //    flavorCell = row.insertCell();
            //    flavorCell.appendChild(inventory[i]);

            //    pricePerCookieCell = row.insertCell();
            //    for (j = 0; j < price.length; j++) {
            //        if (inventory[i] == price[j]) {
            //            priceIndex = j + 1;
            //        }
            //    }
            //    pricePerCookieCell.appendChild(price[priceIndex]);

            //    quantityCell = row.insertCell();
            //    quantityCell.appendChild(inventory[i + 1]);

            //    totalPriceCell = row.insertCell();
            //    totalPricePerItem = parseFloat(price[priceIndex]) * parseFloat(inventory[i + 1]);
            //    totalPriceCell.appendChild(totalPricePerItem);

            //    removeCell = row.insertCell();
            //    removeCell.appendChild("remove");

            //    totalPrice = totalPrice + totalPricePerItem;

            //}


            for (i = 1; i < inventory.length; i = i + 2){
                flavString = "flavor" + i;
                quanString = "quantity" + i;
                priceString = "price" + i;
                totalString = "totalPrice" + i;
                document.getElementById(flavString).innerHTML = inventory[i];
                document.getElementById(quanString).innerHTML = inventory[i + 1];
                for (j = 0; j < price.length; j++) {
                    if (inventory[i] == price[j]) {
                        priceIndex = j + 1;
                    }
                }

                document.getElementById(priceString).innerHTML = "$" + parseFloat(price[priceIndex]).toFixed(2);
                totalPricePerItem = parseFloat(price[priceIndex]) * parseFloat(inventory[i + 1]);
                document.getElementById(totalString).innerHTML = "$" + totalPricePerItem.toFixed(2);
                totalPrice = totalPrice + totalPricePerItem;

                //var row = "row" + i;
                //row.setattribute("hidden", false);

            }


            var taxOnTotal = totalPrice * .10;
            var grandTotal = taxOnTotal + totalPrice + 5;
            

            document.getElementById('totalPrice').innerHTML = "$" + totalPrice.toFixed(2);
            document.getElementById('taxOnTotal').innerHTML = "$" + taxOnTotal.toFixed(2);
            document.getElementById('grandTotal').innerHTML = "$" + grandTotal.toFixed(2);
            document.getElementById('orderItems').innerHTML = http.responseText;

        }
    };

    var userID = getOrLoadUser();
    http.open("GET", "http://localhost:8742/api/values/updateCart/?userID=" + userID, true); // true for asynchronous 
    http.send(null);
}

function getOrLoadUser() {
    //checks to see if a cookie has already been set for the user
    if (get_cookie("userID") == '' || get_cookie("userID") == undefined) {
        set_cookie("userID", uuidv4(), 3); //if there isn't a cookie set for the user, set one
    } 
    var userID = get_cookie("userID");
    document.getElementById('userID').innerHTML = userID;
    return userID;
}

// if you only want to set the cookie for your current domain, you can call it without the third parameter
function set_cookie(cookie_name, cookie_value, lifespan_in_days, valid_domain) { 
    var domain_string = valid_domain ? ("; domain=" + valid_domain) : '';
    document.cookie = cookie_name + "=" + encodeURIComponent(cookie_value) +
        "; max-age=" + 60 * 60 * 24 * lifespan_in_days +
        "; path=/" + domain_string;
} 

//The following function allow you to easily get the cookie value you want by simply specifying the variable name
function get_cookie(cookie_name) {
    var cookie_string = document.cookie;
    if (cookie_string.length != 0) {
        var cookie_value = cookie_string.match('(^|;)[\s]*' + cookie_name + '=([^;]*)');
        return decodeURIComponent(cookie_value[2]);
    }
    return '';
} 

function delete_cookie(cookie_name, valid_domain) {
    // https://www.thesitewizard.com/javascripts/cookies.shtml
    var domain_string = valid_domain ? ("; domain=" + valid_domain) : '';
    document.cookie = cookie_name + "=; max-age=0; path=/" + domain_string;
} 

//creates a UUID
function uuidv4() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g,
        c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}

//this function should:
//1.delete the users cookies
//2.delete the user and the user's cart from the respective dictionaries
//3.send an email to the user and the maufacturer confirming that there was an order placed
function placeOrder() {
    var userID = get_cookie('userID');

    var http = new XMLHttpRequest();
    http.onreadystatechange = function () {
        if (http.readyState === 4) {
            //check to make sure a cart has been created for the user -- if not, throw error message and encourage user to add something to the cart
            if (http.responseText == "") {
                updateCart();
                location.reload(true);
                return;
            } else {
                delete_cookie('userID');
                updateCart();
                location.reload(true);
            }
            document.getElementById('deletedCart').innerHTML = http.responseText;
            //last thing done
           
        }
    };

    http.open("GET", "http://localhost:8742/api/values/placeOrder/?userID=" + userID, true); // true for asynchronous 
    http.send(null);
    //implement email things when time permits.
}



