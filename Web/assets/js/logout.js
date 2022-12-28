$(document).ready(function() {    
    $("#logout").click(function() 
    {
        sessionStorage.setItem("Token", "");    
        sessionStorage.setItem("Id","");
        sessionStorage.setItem("User", "");     
        sessionStorage.setItem("Nome", "");
        window.location.assign("login.html");    
    });
});