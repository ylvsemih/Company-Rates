function getStorageItems() {
    let key = sessionStorage.getItem("SessionKey");
    let validTo = sessionStorage.getItem("ValidTo");
    let userId = sessionStorage.getItem("UserID");
    let email = sessionStorage.getItem("Email");


    let isAdmin = tytPreGetBool("isAdmin");
    let isCompany = tytPreGetBool("isCompany");
    let datetime = new Date(validTo);
    let obj = {
        SessionKey: key,
        ValidTo: datetime,
        UserID: userId,
        isCompany: isCompany,
        isAdmin: isAdmin,
        Email: email
    }
    return obj;
}

function changeLocationIfLoggedIn() {
    let items = getStorageItems();

    if (items.ValidTo > new Date()) {
        window.location = "Profile.html";
    }
}

function changeLocationIfNotLoggedIn() {
    let items = getStorageItems();
    if (items.ValidTo < new Date() || items.SessionKey == undefined) {
        window.location = "Login.html";
    }
}

function showLoggedInUser() {
    let items = getStorageItems();
    if (items.ValidTo > new Date()) {

        $('#ul_profile > li').hide();
        var liRow = `<li><a id="link_profile" href="Profile.html?userid=${items.UserID}"><span class="glyphicon glyphicon-user"></span> Hey, ${items.Email}</a></li>`;
        $('#ul_profile').append(liRow);

    }
    else {
        $('#ul_profile > li').hide();
        var liRow1 = '<li><a href="Registration.html"><span class="glyphicon glyphicon-user"></span>Registration</a></li>';
        var liRow2 = '<li><a href="Login.html"><span class="glyphicon glyphicon-log-in"></span>Login</a></li>';
        $('#ul_profile').append(liRow1);
        $('#ul_profile').append(liRow2);
    }
}

async function sha256(message) {
    // encode as UTF-8
    const msgBuffer = new TextEncoder('utf-8').encode(message);

    // hash the message
    const hashBuffer = await crypto.subtle.digest('SHA-256', msgBuffer);

    // convert ArrayBuffer to Array
    const hashArray = Array.from(new Uint8Array(hashBuffer));

    // convert bytes to hex string                  
    const hashHex = hashArray.map(b => ('00' + b.toString(16)).slice(-2)).join('');
    return hashHex;
}

function tytPreGetBool(pre) {
    return sessionStorage.getItem(pre) === 'true'
}