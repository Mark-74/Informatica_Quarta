patterns = {
    "username" : new RegExp('^[a-zA-Z0-9_\-]{8,64}$'),
    "password" : new RegExp('^(?=.{8,64}$)[a-z]+[A-Z]+[0-9]+\W+$')
}

function check(id){
    const tag = $(`#${id}`);
    if (!patterns[id].test(tag.val()))
        tag.removeClass('text-white').addClass('text-danger');
    else
        tag.removeClass('text-danger').addClass('text-white');
}

function linkAll(){
    $("#username").on("change", () => {check("username")});
    $("#password").on("change", () => {check("password")});
}

$( window ).on( "load", function() {
    linkAll()
  } );