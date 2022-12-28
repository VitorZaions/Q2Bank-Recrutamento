function BtnLoading(elem) {
    $(elem).attr("data-original-text", $(elem).html());
    $(elem).prop("disabled", true);
    $(elem).html('<i class="spinner-border spinner-border-sm"></i> Registrando..');
}

function BtnReset(elem) {
    $(elem).prop("disabled", false);
    $(elem).html($(elem).attr("data-original-text"));
}

var IDRecebido = 0;
var NomeEmpresa = null;
var ObjetoRecebido = null;

$(document).ready(function() {

    var url   = window.location.search.replace("?", "");
    var itens = url.split("&");

    if(itens.length < 2){
        window.location.assign("index.html"); 
    }
    
    IDRecebido = itens[0].slice(3); // 3 pois id= tem 3 caracteres
    NomeEmp = itens[1].slice(5);

    if(IDRecebido == ""){
        window.location.assign("index.html"); 
    }

    if(NomeEmp == "")
    {
        window.location.assign("index.html"); 
    }    

    NomeEmpresa = decodeURIComponent(NomeEmp); // 5 pois nome= tem 5 caracteres  
    $('#tituloempresa').text("Funcionários ("+ NomeEmpresa +")");

    $("#novo_funcionario").click(function() 
    {
        window.location.assign("funcionario.html?id="+IDRecebido+"&nome=" + encodeURIComponent(NomeEmpresa)); 
    });  

     $(document).on('click', ".deletefunc", function (event) {
        event.preventDefault();

        var element =  event.currentTarget;        
        var IDFuncionario = event.currentTarget.id;        

        ele = element.closest('button');

        $.ajax({
            type: 'DELETE',
            url: 'https://' + host + ':'+ porta +'/funcionario/deletar',
            headers: {Authorization: Final},
            async: false,
            data: JSON.stringify(IDFuncionario),
            success: function (data) 
            { 
                element.closest('tr').remove();
                $('#text_modal').html("Funcionário removido com sucesso!</a>");
                $('#ModalErrorLabel').text('Sucesso');
                $('#ModalError').modal('show');
                         
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
                    $('#text_modal').html("Não foi possível remover o funcionário, tente novamente.");
                    $('#ModalError').modal('show');
                }
    
                //BtnReset($this);                
            },
            contentType: "application/json",
            dataType: 'json'
        });


    });

     
    $(document).on('click', ".editfunc", function (event) {
        event.preventDefault();
        var IDFuncionario = event.currentTarget.id;          
        window.location.assign("funcionarioeditar.html?id=" + IDFuncionario+ "&idempresa=" + IDRecebido + "&nome="+encodeURIComponent(NomeEmpresa));
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
        url: 'https://' + host + ':'+ porta +'/funcionario/listar',
        headers: {Authorization: Final},
        data: {IDEmpresa: IDRecebido},
        async: false,
        success: function (data) 
        { 

            for (var i = 0; i < data.length; i++) {
                // Iterate over numeric indexes from 0 to 5, as everyone expects.
                console.log(data[i]);

                var html = "<ul class='list-inline m-0'><li class='list-inline-item editfunc' id='"+ data[i].id +"'><button class='btn btn-success btn-sm rounded-0' type='button' data-toggle='tooltip' data-placement='top' title='Editar'><i class='fa fa-edit'></i></button></li><li class='deletefunc list-inline-item' id='"+ data[i].id +"'><button class='btn btn-danger btn-sm rounded-0' type='button' data-toggle='tooltip' data-placement='top' title='Deletar'><i class='fa fa-trash'></i></button></li></ul>"

                $("#TabelaFuncionarios > tbody").append("<tr> <td>" + data[i].nome +"</td> <td> " + data[i].cargo + "</td> <td>" + html + "</td> </tr>");
            }

            
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) 
        { 
            if(XMLHttpRequest.status != 404)
            {
                window.location.assign("index.html");  
            }      
        },
        contentType: "application/json",
        dataType: 'json'
      });  
    

    document.getElementsByTagName("html")[0].style.visibility = "visible";



});