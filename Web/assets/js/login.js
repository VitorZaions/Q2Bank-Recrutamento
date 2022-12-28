function BtnLoading(elem) {
    $(elem).attr("data-original-text", $(elem).html());
    $(elem).prop("disabled", true);
    $(elem).html('<i class="spinner-border spinner-border-sm"></i> Logando..');
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

    $("#logar").click(function() 
    {
   
        var Usuario = {
            User: $("#username").val(),
            Senha: $("#password").val()
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
    
    
        var $this = $(this);
    
        BtnLoading($this);
        
        JSON.stringify(Usuario)
        
        $.ajax({
            type: 'POST',
            url: 'https://' + host + porta +'/usuario/login',
            headers: {},
            data: JSON.stringify(Usuario),
            success: function (data) 
            { 
                $('#text_modal').html("Login Efetuado com sucesso!</a>");
                $('#ModalErrorLabel').text('Sucesso');
                $('#ModalError').modal('show')
                BtnReset($this); 
                
                console.log(data);

                sessionStorage.setItem("Token", data.token);    
                sessionStorage.setItem("Id", data.user.id);
                sessionStorage.setItem("User", data.user.user);     
                sessionStorage.setItem("Nome", data.user.nome);
                window.location.assign("index.html");               
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) 
            {  
                if(XMLHttpRequest.status == 400 || XMLHttpRequest.status == 404)
                {
                    $('#ModalErrorLabel').text('Aviso');
                    $('#text_modal').html(XMLHttpRequest.responseText);
                    $('#ModalError').modal('show');
                }
                else
                {
                    $('#ModalErrorLabel').text('Aviso');
                    $('#text_modal').html("Não foi possível realizar o login, tente novamente.");
                    $('#ModalError').modal('show');
                }
    
                BtnReset($this);                
            },
            contentType: "application/json",
            dataType: 'json'
          });
    
       
        // Save data to the current session's store
       /* sessionStorage.setItem("username", "John");
        // Access some stored data
        alert( "username = " + sessionStorage.getItem("username"));
    
        //Consulta o login
        $.getJSON("https://viacep.com.br/ws/"+ cep +"/json/?callback=?", function(dados) {
    
                    if (!("erro" in dados)) {
                        //Atualiza os campos com os valores da consulta.
                        $("#rua").val(dados.logradouro);
                        $("#bairro").val(dados.bairro);
                        $("#cidade").val(dados.localidade);
                        $("#uf").val(dados.uf);
                        $("#ibge").val(dados.ibge);
                    } //end if.
                    else {
                        //CEP pesquisado não foi encontrado.
                        limpa_formulário_cep();
                        alert("CEP não encontrado.");
                    }
                });
    
    
        //Verifica se campo cep possui valor informado.
        if (cep != "") {
    
            //Expressão regular para validar o CEP.
            var validacep = /^[0-9]{8}$/;
    
            //Valida o formato do CEP.
            if(validacep.test(cep)) {
    
                //Preenche os campos com "..." enquanto consulta webservice.
                $("#rua").val("...");
                $("#bairro").val("...");
                $("#cidade").val("...");
                $("#uf").val("...");
                $("#ibge").val("...");
    
                //Consulta o webservice viacep.com.br/
                $.getJSON("https://viacep.com.br/ws/"+ cep +"/json/?callback=?", function(dados) {
    
                    if (!("erro" in dados)) {
                        //Atualiza os campos com os valores da consulta.
                        $("#rua").val(dados.logradouro);
                        $("#bairro").val(dados.bairro);
                        $("#cidade").val(dados.localidade);
                        $("#uf").val(dados.uf);
                        $("#ibge").val(dados.ibge);
                    } //end if.
                    else {
                        //CEP pesquisado não foi encontrado.
                        limpa_formulário_cep();
                        alert("CEP não encontrado.");
                    }
                });
            } //end if.
            else {
                //cep é inválido.
                limpa_formulário_cep();
                alert("Formato de CEP inválido.");
            }
        } //end if.
        else {
            //cep sem valor, limpa formulário.
            limpa_formulário_cep();
        }*/
    });

    document.getElementsByTagName("html")[0].style.visibility = "visible";

});