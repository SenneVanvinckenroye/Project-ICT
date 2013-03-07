/* File Created: maart 6, 2013 */


$(document).ready(function () {//Onload jquery func
    // Handler for .ready() called.
    $(".login").hide(); //hide elementen in klasse 'page2'
    //change input field html code
});

function gotoField(veld) {
    switch(veld) 
    {
        case'login':
        {
            $(".login").show(); //toon elementen in klasse 'page2'
            //change input field html code
        }
        case'register':
        {
            $(".register").show(); //toon elementen in klasse 'page2'
        }
    }
    
}