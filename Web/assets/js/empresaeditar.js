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
    IDRecebido = itens[0].slice(3); // 5 pois nome= tem 5 caracteres
    
    if(IDRecebido == ""){
        window.location.assign("index.html"); 
    }

    var SPMaskBehavior = function (val) 
    {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
    spOptions = 
    {
        onKeyPress: function(val, e, field, options) 
        {
                        field.mask(SPMaskBehavior.apply({}, arguments), options);
        }
    };

    $('#telefone').mask(SPMaskBehavior, spOptions);
    $('#cep').mask('00000-000', {reverse: true});

    // Não deixa caracteres especiais no nome.
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

    $(".backbutton").click(function() 
    {
        window.location="index.html";
    });

    function limpa_formulário_cep() {
        // Limpa valores do formulário de cep.
        $("#rua").val("");
        $("#bairro").val("");
        $("#cidade").val("");
        $("#uf").val("");
        //$("#ibge").val("");
    }

    $("#cep").blur(function() {

        //Nova variável "cep" somente com dígitos.
        var cep = $(this).val().replace(/\D/g, '');

        //Verifica se campo cep possui valor informado.
        if (cep != "") {

            //Expressão regular para validar o CEP.
            var validacep = /^[0-9]{8}$/;

            //Valida o formato do CEP.
            if(validacep.test(cep)) {

                document.getElementById('cep').value = cep.substring(0,5)
                +"-"
                +cep.substring(5);

                //Preenche os campos com "..." enquanto consulta webservice.
                $("#rua").val("...");
                $("#bairro").val("...");
                $("#cidade").val("...");
                $("#uf").val("...");
                //$("#ibge").val("...");

                //Consulta o webservice viacep.com.br/
                $.getJSON("https://viacep.com.br/ws/"+ cep +"/json/?callback=?", function(dados) {

                    if (!("erro" in dados)) {
                        //Atualiza os campos com os valores da consulta.
                        $("#rua").val(dados.logradouro);
                        $("#bairro").val(dados.bairro);
                        $("#cidade").val(dados.localidade);
                        $("#uf").val(dados.uf);
                       // $("#ibge").val(dados.ibge);
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
        }
    });

    $("#salvar").click(function() 
    {

        IdUser = sessionStorage.getItem("Id");

        if(IdUser == "")
        {
            LimparConfig();
            window.location.assign("login.html"); 
        }

        var Empresa = {
            Id: ObjetoRecebido.id,
            Nome: $("#nome").val(),
            UF: $("#uf").val(),
            CEP: $("#cep").val().replace(/\D/g, ''),
            Logradouro: $("#rua").val(),
            Localidade: $("#cidade").val(),
            Bairro: $("#bairro").val(),
            Numero: $("#numero").val(),
            Complemento: $("#complemento").val(),
            Telefone: $("#telefone").val().replace(/\D/g, ''),
            UsuarioId: ObjetoRecebido.usuarioId
          }  
    
       
        if (Empresa.Nome == "") 
        {
            $('#text_modal').html('Insira o nome para continuar.');
            $('#ModalErrorLabel').text('Aviso');
            $('#ModalError').modal('show');
            return;
        }
    
        if(Empresa.CEP == ""){
            $('#text_modal').html('Insira a CEP para continuar.');
            $('#ModalErrorLabel').text('Aviso');
            $('#ModalError').modal('show');
            return;
        }

        if(Empresa.CEP.length < 8){
            $('#text_modal').html('O CEP deve conter 8 dígitos.');
            $('#ModalErrorLabel').text('Aviso');
            $('#ModalError').modal('show');
            return;
        }

        if(Empresa.UF == ""){
            $('#text_modal').html('Insira a UF para continuar.');
            $('#ModalErrorLabel').text('Aviso');
            $('#ModalError').modal('show');
            return;
        }

        if(Empresa.Logradouro == ""){
            $('#text_modal').html('Insira o logradouro para continuar.');
            $('#ModalErrorLabel').text('Aviso');
            $('#ModalError').modal('show');
            return;
        }
    
        if(Empresa.Localidade == ""){
            $('#text_modal').html('Insira a localidade para continuar.');
            $('#ModalErrorLabel').text('Aviso');
            $('#ModalError').modal('show');
            return;
        }
    
        if(Empresa.Bairro == ""){
            $('#text_modal').html('Insira o bairro para continuar.');
            $('#ModalErrorLabel').text('Aviso');
            $('#ModalError').modal('show');
            return;
        }

        if(Empresa.Numero == ""){
            $('#text_modal').html('Insira o número para continuar.');
            $('#ModalErrorLabel').text('Aviso');
            $('#ModalError').modal('show');
            return;
        }

        /*
        if(Empresa.Complemento == ""){
            $('#text_modal').html('Insira o complemento para continuar.');
            $('#ModalErrorLabel').text('Aviso');
            $('#ModalError').modal('show');
            return;
        }*/

        if(Empresa.Telefone == ""){
            $('#text_modal').html('Insira o telefone.');
            $('#ModalErrorLabel').text('Aviso');
            $('#ModalError').modal('show');
            return;
        }
        
        if(Empresa.Telefone.length < 10){
            $('#text_modal').html('O telefone deve conter pelo menos 10 dígitos.');
            $('#ModalErrorLabel').text('Aviso');
            $('#ModalError').modal('show');
            return;
        }

        var $this = $(this);
    
        BtnLoading($this);     
        
        User = sessionStorage.getItem("User");
        Token = sessionStorage.getItem("Token");
    
        if(User == "" || Token == "")
        {
            window.location.assign("index.html");
        }
    
        Final = "Bearer " + Token;
               
        $.ajax({
            type: 'PUT',
            url: 'https://' + host + porta +'/empresa/atualizar',
            headers: {Authorization: Final},
            data: JSON.stringify(Empresa),
            success: function (data) 
            { 
                $('#text_modal').html("Empresa atualizada com sucesso!</a>");
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
                    LimparConfig();
                    window.location.assign("login.html"); 
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

    User = sessionStorage.getItem("User");
    Token = sessionStorage.getItem("Token");

    if(User == "" || Token == "")
    {
        window.location.assign("index.html");
    }

    Final = "Bearer " + Token;

    //IDEmpresa = IDRecebido;

    $.ajax({
        type: 'GET',
        url: 'https://' + host + porta +'/empresa/obter',
        headers: {Authorization: Final},
        data: {IDEmpresa: IDRecebido},
        async: false,
        success: function (data) 
        { 
            ObjetoRecebido = data;
            $("#nome").val(data.nome);
            $("#uf").val(data.uf);
            $("#cep").val(data.cep);
            $("#rua").val(data.logradouro);
            $("#cidade").val(data.localidade);
            $("#bairro").val(data.bairro);
            $("#numero").val(data.numero);
            $("#complemento").val(data.complemento);
            $("#telefone").val(data.telefone);
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