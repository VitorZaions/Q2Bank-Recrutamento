function BtnLoading(elem) {
    $(elem).attr("data-original-text", $(elem).html());
    $(elem).prop("disabled", true);
    $(elem).html('<i class="spinner-border spinner-border-sm"></i> Salvando..');
}

function BtnReset(elem) {
    $(elem).prop("disabled", false);
    $(elem).html($(elem).attr("data-original-text"));
}

var IDRecebido = 0;
var ObjetoRecebido = null;

$(document).ready(function() {
    
    var url   = window.location.search.replace("?", "");
    var itens = url.split("&");
    IDRecebido = itens[0].slice(3); // 3 pois id= tem 3 caracteres
    
    $(".backbutton").click(function() 
    {
        window.location="index.html";
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

    $("#salvar").click(function() 
    {
        _User = sessionStorage.getItem("User");

        var Usuario = {
            Senha: $("#password").val(),
            Nome: $("#nome").val(),
            User: _User
        }    

        if(Usuario.Nome == ""){
            $('#text_modal').html('Insira o nome completo para continuar.');
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

        var $this = $(this);
    
        BtnLoading($this);     
        
        Token = sessionStorage.getItem("Token");
    
        if(_User == "" || Token == "")
        {
            window.location.assign("index.html");
        }
    
        Final = "Bearer " + Token;
               
        $.ajax({
            type: 'PUT',
            url: 'https://' + host + porta +'/usuario/atualizar',
            headers: {Authorization: Final},
            data: JSON.stringify(Usuario),
            crossDomain: true,
            success: function (data) 
            {                 
                $('#text_modal').html("Usuário atualizado com sucesso!");
                $('#ModalErrorLabel').text('Sucesso');
                $('#ModalError').modal('show')
                BtnReset($this);                 
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) 
            {  
                if(XMLHttpRequest.status == 400 || XMLHttpRequest.status == 404)
                {
                    $('#ModalErrorLabel').text('Aviso');
                    $('#text_modal').html(XMLHttpRequest.responseText);
                    $('#ModalError').modal('show');
                }
                else if(XMLHttpRequest.status == 401)
                {
                    //LimparConfig();
                   // window.location.assign("login.html"); 
                }
                else
                {
                    $('#ModalErrorLabel').text('Aviso');
                    $('#text_modal').html("Não foi possível criar a empresa, tente novamente.");
                    $('#ModalError').modal('show');
                }
    
                BtnReset($this);                
            },
            contentType: "application/json",
            dataType: 'json'
        });

    });

    _User = sessionStorage.getItem("User");
    Token = sessionStorage.getItem("Token");

    if(_User == "" || Token == "")
    {
        window.location.assign("index.html");
    }

    Final = "Bearer " + Token;

    $.ajax({
        type: 'GET',
        url: 'https://' + host + porta +'/usuario/obter',
        headers: {Authorization: Final},
        async: false,
        data: {},
        crossDomain: true,
        success: function (data) 
        { 
            ObjetoRecebido = data;
            $("#nome").val(data.nome);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) 
        {  
            if(XMLHttpRequest.status == 404 || XMLHttpRequest.status == 401 || XMLHttpRequest.status == 400)
            {
                window.location.assign("index.html");   
            }
            else
            {
                LimparConfig();
                window.location.assign("login.html");     
            }      
        },
        contentType: "application/json",
        dataType: 'json'
      });  

      document.getElementsByTagName("html")[0].style.visibility = "visible";



});