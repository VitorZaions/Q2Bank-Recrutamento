function BtnLoading(elem) {
    $(elem).attr("data-original-text", $(elem).html());
    $(elem).prop("disabled", true);
    $(elem).html('<i class="spinner-border spinner-border-sm"></i> Registrando..');
}

function BtnReset(elem) {
    $(elem).prop("disabled", false);
    $(elem).html($(elem).attr("data-original-text"));
}

$(document).ready(function() {

    $('#username').on('input', function() {
        var c = this.selectionStart,
            r = /[&?]/,
            v = $(this).val();
        if(r.test(v)) {
          $(this).val(v.replace(r, ''));
          c--;
        }
        this.setSelectionRange(c, c);
      });

      $('#nome').on('input', function() {
        var c = this.selectionStart,
            r = /[&?]/,
            v = $(this).val();
        if(r.test(v)) {
          $(this).val(v.replace(r, ''));
          c--;
        }
        this.setSelectionRange(c, c);
      });


    //Quando o campo cep perde o foco.
    $("#registrar").click(function() {

    //$('#ModalError').modal('show')
    //return;
    //Nova variável "cep" somente com dígitos.

    var Usuario = {
        User: $("#username").val(),
        Senha: $("#password").val(),
        Nome: $("#nome").val()
    }


    if (Usuario.User == "") 
    {
        $('#text_modal').html('Insira o login para continuar.');
        $('#ModalErrorLabel').text('Aviso');
        $('#ModalError').modal('show');
        return;
    }

    if(Usuario.Senha == ""){
        $('#text_modal').html('Insira a senha para continuar.');
        $('#ModalErrorLabel').text('Aviso');
        $('#ModalError').modal('show');
        return;
    }

    if($("#password2").val() == ""){
        $('#text_modal').html('Insira a sua confirmação de senha para continuar.');
        $('#ModalErrorLabel').text('Aviso');
        $('#ModalError').modal('show');
        return;
    }

    if(Usuario.Senha != $("#password2").val())
    {
        console.log("senha 1 :" + pass);
        console.log("senha 2 :" + pass2);
        $('#text_modal').html('Senha e confirmação de senha diferentes.');
        $('#ModalErrorLabel').text('Aviso');
        $('#ModalError').modal('show');
        return;
    }

    if(Usuario.Nome == ""){
        $('#text_modal').html('Insira o nome completo para continuar.');
        $('#ModalErrorLabel').text('Aviso');
        $('#ModalError').modal('show');
        return;
    }

    // Save data to the current session's store
    //sessionStorage.setItem("username", "John");
    // Access some stored data
    //alert( "username = " + sessionStorage.getItem("username"));

    //Tenta Realizar o registro
    var $this = $(this);

    BtnLoading($this);
    
    JSON.stringify(Usuario)
    
    $.ajax({
        type: 'POST',
        url: 'https://' + host + porta +'/usuario/registro',
        headers: {},
        data: JSON.stringify(Usuario),
        crossDomain: true,
        success: function (data) 
        { 
            $('#text_modal').html("Conta criada com sucesso, você já pode <a class='text-muted' href='login.html'>realizar login.</a>");
            $('#ModalErrorLabel').text('Sucesso');
            $('#ModalError').modal('show');
            BtnReset($this);  
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) 
        {  
            if(XMLHttpRequest.status == 400)
            {
                $('#ModalErrorLabel').text('Aviso');
                $('#text_modal').html(XMLHttpRequest.responseText);
                $('#ModalError').modal('show');
            }
            else
            {
                $('#ModalErrorLabel').text('Aviso');
                $('#text_modal').html("Não foi possível criar a conta, tente novamente.");
                $('#ModalError').modal('show');
            }

            BtnReset($this);                
        },
        contentType: "application/json",
        dataType: 'json'
      });
   
    });    


    document.getElementsByTagName("html")[0].style.visibility = "visible";

});