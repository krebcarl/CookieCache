function runTests() {
    document.getElementById('writeTestTextHere').innerHTML += "<br/>" + "Starting Tests now!" + "<br/>";
    test1(); //checks that peanut butter jam cookies can be added to the cart
    test2(); //checks that chocolate java chip cookies can be added to the cart
    test3(); //checks that MM Madness cookies can be added to the cart
    test4(); //checks that LRU BLU-berry cookies can be added to the cart 
    test5(); //Testing that a valid quantity (0 case) is required 
    test6(); //Testing that a valid quantity (null case) is required 
    test7(); //Testing that a valid quantity (negative case) is required 
    //test8();
    //test9();
}

function test1() {
    var flavor = 'Peanut Butter Jam';
    var quantity = 500;
    var userID = 'uniqueGUID';
    var nextLine;
    var http = new XMLHttpRequest();
    http.onreadystatechange = function () {
        if (http.readyState === 4) {
            if (http.responseText ==
                ("Stateless Service hit | " + "Thank you! " + quantity + " " + flavor + " cookies have been added to your cart!")) {
                document.getElementById('writeTestTextHere').innerHTML += "Testing Peanut Butter Cookies are added: ";
                nextLine = "<span style='color:#1eb23e'>" + "Passed" + "</span>";
                document.getElementById('writeTestTextHere').innerHTML += nextLine;
                document.getElementById('writeTestTextHere').innerHTML += "<br/>";
            } else {
                document.getElementById('writeTestTextHere').innerHTML += "Testing Peanut Butter Cookies are added: ";
                nextLine = "<span style='color:#db0404'>" + "Failed" + "</span>";
                document.getElementById('writeTestTextHere').innerHTML += nextLine;
                document.getElementById('writeTestTextHere').innerHTML += "<br/>";
            }
        }
    };

    http.open("GET", "http://localhost:8742/api/values/AddToCart/?flavor=" + flavor + "&quantity=" + quantity + "&userID=" + userID, true); // true for asynchronous 
    http.send(null);
}

function test2() {
    var flavor = 'Chocolate Java Chip';
    var quantity = 500;
    var userID = 'uniqueGUID';
    var nextLine;
    var http = new XMLHttpRequest();
    http.onreadystatechange = function () {
        if (http.readyState === 4) {
            if (http.responseText ==
                ("Stateless Service hit | " + "Thank you! " + quantity + " " + flavor + " cookies have been added to your cart!")) {
                document.getElementById('writeTestTextHere').innerHTML += "Testing Chocolate Java Chip Cookies are added: ";
                nextLine = "<span style='color:#1eb23e'>" + "Passed" + "</span>";
                document.getElementById('writeTestTextHere').innerHTML += nextLine;
                document.getElementById('writeTestTextHere').innerHTML += "<br/>";
            } else {
                document.getElementById('writeTestTextHere').innerHTML += "Testing Chocolate Java Chip Cookies are added: ";
                nextLine = "<span style='color:#db0404'>" + "Failed" + "</span>";
                document.getElementById('writeTestTextHere').innerHTML += nextLine;
                document.getElementById('writeTestTextHere').innerHTML += "<br/>";
            }
        }
    };

    http.open("GET", "http://localhost:8742/api/values/AddToCart/?flavor=" + flavor + "&quantity=" + quantity + "&userID=" + userID, true); // true for asynchronous 
    http.send(null);
}

function test3() {
    var flavor = 'MM Madness';
    var quantity = 500;
    var userID = 'uniqueGUID';
    var nextLine;
    var http = new XMLHttpRequest();
    http.onreadystatechange = function () {
        if (http.readyState === 4) {
            if (http.responseText ==
                ("Stateless Service hit | " + "Thank you! " + quantity + " " + flavor + " cookies have been added to your cart!")) {
                document.getElementById('writeTestTextHere').innerHTML += "Testing MM Madness Cookies are added: ";
                nextLine = "<span style='color:#1eb23e'>" + "Passed" + "</span>";
                document.getElementById('writeTestTextHere').innerHTML += nextLine;
                document.getElementById('writeTestTextHere').innerHTML += "<br/>";
            } else {
                document.getElementById('writeTestTextHere').innerHTML += "Testing MM Madness Cookies are added: ";
                nextLine = "<span style='color:#db0404'>" + "Failed" + "</span>";
                document.getElementById('writeTestTextHere').innerHTML += nextLine;
                document.getElementById('writeTestTextHere').innerHTML += "<br/>";
            }
        }
    };

    http.open("GET", "http://localhost:8742/api/values/AddToCart/?flavor=" + flavor + "&quantity=" + quantity + "&userID=" + userID, true); // true for asynchronous 
    http.send(null);
}

function test4() {
    var flavor = 'LRU BLU-berry';

    var quantity = 500;
    var userID = 'uniqueGUID';
    var nextLine;
    var http = new XMLHttpRequest();
    http.onreadystatechange = function () {
        if (http.readyState === 4) {
            if (http.responseText ==
                ("Stateless Service hit | " + "Thank you! " + quantity + " " + flavor + " cookies have been added to your cart!")) {
                document.getElementById('writeTestTextHere').innerHTML += "Testing LRU BLU-berry Cookies are added: ";
                nextLine = "<span style='color:#1eb23e'>" + "Passed" + "</span>";
                document.getElementById('writeTestTextHere').innerHTML += nextLine;
                document.getElementById('writeTestTextHere').innerHTML += "<br/>";
            } else {
                document.getElementById('writeTestTextHere').innerHTML += "Testing LRU BLU-berry Cookies are added: ";
                nextLine = "<span style='color:#db0404'>" + "Failed" + "</span>";
                document.getElementById('writeTestTextHere').innerHTML += nextLine;
                document.getElementById('writeTestTextHere').innerHTML += "<br/>";
            }
        }
    };

    http.open("GET", "http://localhost:8742/api/values/AddToCart/?flavor=" + flavor + "&quantity=" + quantity + "&userID=" + userID, true); // true for asynchronous 
    http.send(null);
}

function test5() {
    var flavor = 'LRU BLU-berry';
    var quantity = 0;
    var nextLine;
  
    if (addToCart(flavor, quantity) == "Not accurate quantity") {
        document.getElementById('writeTestTextHere').innerHTML += "Testing that a valid quantity (0 case) is required: ";
        nextLine = "<span style='color:#1eb23e'>" + "Passed" + "</span>";
        document.getElementById('writeTestTextHere').innerHTML += nextLine;
        document.getElementById('writeTestTextHere').innerHTML += "<br/>";
    } else {
        document.getElementById('writeTestTextHere').innerHTML += "Testing that a valid quantity (0 case) is required: ";
        nextLine = "<span style='color:#db0404'>" + "Failed" + "</span>";
        document.getElementById('writeTestTextHere').innerHTML += nextLine;
        document.getElementById('writeTestTextHere').innerHTML += "<br/>";
    }
}

function test6() {
    var flavor = 'LRU BLU-berry';
    var quantity = null;
    var nextLine;

    if (addToCart(flavor, quantity) == "Not accurate quantity") {
        document.getElementById('writeTestTextHere').innerHTML += "Testing that a valid quantity (null case) is required: ";
        nextLine = "<span style='color:#1eb23e'>" + "Passed" + "</span>";
        document.getElementById('writeTestTextHere').innerHTML += nextLine;
        document.getElementById('writeTestTextHere').innerHTML += "<br/>";
    } else {
        document.getElementById('writeTestTextHere').innerHTML += "Testing that a valid quantity (null case) is required: ";
        nextLine = "<span style='color:#db0404'>" + "Failed" + "</span>";
        document.getElementById('writeTestTextHere').innerHTML += nextLine;
        document.getElementById('writeTestTextHere').innerHTML += "<br/>";
    }
}

function test7() {
    var flavor = 'LRU BLU-berry';
    var quantity = -1;
    var nextLine;

    if (addToCart(flavor, quantity) == "Not accurate quantity") {
        document.getElementById('writeTestTextHere').innerHTML += "Testing that a valid quantity (negative case) is required: ";
        nextLine = "<span style='color:#1eb23e'>" + "Passed" + "</span>";
        document.getElementById('writeTestTextHere').innerHTML += nextLine;
        document.getElementById('writeTestTextHere').innerHTML += "<br/>";
    } else {
        document.getElementById('writeTestTextHere').innerHTML += "Testing that a valid quantity (negative case) is required: ";
        nextLine = "<span style='color:#db0404'>" + "Failed" + "</span>";
        document.getElementById('writeTestTextHere').innerHTML += nextLine;
        document.getElementById('writeTestTextHere').innerHTML += "<br/>";
    }
}

//function test8() {
//    var cookieName = "testCookie";
//    var nextLine;
//    set_cookie("testCookie", "testCookieValue", 3);
//    var cookieTest = get_cookie(cookieName);

//    if (cookieTest != '') {
//        document.getElementById('writeTestTextHere').innerHTML += "Testing that if a cookie is set the correct value is returned when get_cookie is called: ";
//        nextLine = "<span style='color:#1eb23e'>" + "Passed" + "</span>";
//        document.getElementById('writeTestTextHere').innerHTML += nextLine;
//        document.getElementById('writeTestTextHere').innerHTML += "<br/>";
//    } else {
//        document.getElementById('writeTestTextHere').innerHTML += "Testing that if a cookie is set the correct value is returned when get_cookie is called: ";
//        nextLine = "<span style='color:#db0404'>" + "Failed" + "</span>";
//        document.getElementById('writeTestTextHere').innerHTML += nextLine;
//        document.getElementById('writeTestTextHere').innerHTML += "<br/>";
//    }
//}

//function test9() {
//    var nextLine;
//    var cookieTest = get_cookie("noCookieShouldBeSet");

//    if (cookieTest == '') {
//        document.getElementById('writeTestTextHere').innerHTML += "Testing that if a cookie is not set that a value shouldn't be returned when get_cookie is called: ";
//        nextLine = "<span style='color:#1eb23e'>" + "Passed" + "</span>";
//        document.getElementById('writeTestTextHere').innerHTML += nextLine;
//        document.getElementById('writeTestTextHere').innerHTML += "<br/>";
//    } else {
//        document.getElementById('writeTestTextHere').innerHTML += "Testing that if a cookie is not set that a value shouldn't be returned when get_cookie is called: ";
//        nextLine = "<span style='color:#db0404'>" + "Failed" + "</span>";
//        document.getElementById('writeTestTextHere').innerHTML += nextLine;
//        document.getElementById('writeTestTextHere').innerHTML += "<br/>";
//    }
//}

