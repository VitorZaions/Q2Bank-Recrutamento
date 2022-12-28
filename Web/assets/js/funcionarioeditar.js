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
var IDRecebidoEmpresa = 0;
var ObjetoRecebido = null;
var NomeEmpresa = null;

$(document).ready(function() {
    
    var url   = window.location.search.replace("?", "");
    var itens = url.split("&");

    if(itens.length < 3){
        window.location.assign("index.html"); 
    }
    
    IDRecebido = itens[0].slice(3); // 3 pois id= tem 3 caracteres
    IDRecebidoEmpresa = itens[1].slice(10); // 10 pois idempresa= tem 10 caracteres   
    NomeEmp = itens[2].slice(5); // 5 pois nome= tem 5 caracteres

    if(IDRecebido == ""){
        window.location.assign("index.html"); 
    }

    if(IDRecebidoEmpresa == "")
    {
        window.location.assign("index.html"); 
    }

    if(NomeEmp == "")
    {
        window.location.assign("index.html"); 
    }

    NomeEmpresa = decodeURIComponent(NomeEmp); // 5 pois nome= tem 5 caracteres  

    $(".backbutton").click(function() 
    {
        window.location="funcionarios.html?id=" + IDRecebidoEmpresa + "&nome="+NomeEmpresa;
    });

    $("#salario").maskMoney({
        prefix: "R$",
        decimal: ",",
        thousands: "."
    });

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

    $("#salvar").click(function() 
    {

        IdUser = sessionStorage.getItem("Id");

        if(IdUser == "")
        {
            LimparConfig();
            window.location.assign("login.html"); 
        }

        var Funcionario = {
            Id: IDRecebido,
            Nome: $("#nome").val(),
            Salario: $("#salario").val().replace(/\D/g, ''),
            Cargo: $("#cargo").val(),
            EmpresaID: IDRecebidoEmpresa
        }
    
       
        if (Funcionario.Nome == "") 
        {
            $('#text_modal').html('Insira o nome para continuar.');
            $('#ModalErrorLabel').text('Aviso');
            $('#ModalError').modal('show');
            return;
        }
    
        if($("#salario").val() == ""){
            $('#text_modal').html('Insira a salário para continuar.');
            $('#ModalErrorLabel').text('Aviso');
            $('#ModalError').modal('show');
            return;
        }

        if(Funcionario.Salario == 0){
            $('#text_modal').html('Insira um salário maior que zero.');
            $('#ModalErrorLabel').text('Aviso');
            $('#ModalError').modal('show');
            return;
        }

        if(Funcionario.Cargo == ""){
            $('#text_modal').html('Insira o cargo.');
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
            url: 'https://' + host + ':'+ porta +'/funcionario/atualizar',
            headers: {Authorization: Final},
            data: JSON.stringify(Funcionario),
            success: function (data) 
            { 
                $('#text_modal').html("Funcionário atualizado com sucesso!</a>");
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
                    $('#text_modal').html("Não foi possível atualizar o funcionário, tente novamente.");
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

    $.ajax({
        type: 'GET',
        url: 'https://' + host + ':'+ porta +'/funcionario/obter',
        headers: {Authorization: Final},
        data: {IDFuncionario: IDRecebido},
        async: false,
        success: function (data) 
        { 
            $("#nome").val(data.nome);
            $("#salario").val(data.salario);
            $("#cargo").val(data.cargo);           
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

      $('#salario').focus();
      $('#salario').blur();

      document.getElementsByTagName("html")[0].style.visibility = "visible";

});