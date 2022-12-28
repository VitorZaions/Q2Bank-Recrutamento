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

    $("#nova_empresa").click(function() 
    {
        window.location.assign("empresa.html"); 
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
        url: 'https://' + host + ':'+ porta +'/empresa/listar',
        async: false,
        headers: {Authorization: Final},
        data: {},
        success: function (data) 
        { 

            for (var i = 0; i < data.length; i++) {
                // Iterate over numeric indexes from 0 to 5, as everyone expects.
                console.log(data[i]);


                var html = "<ul class='list-inline m-0'><li class='list-inline-item editemp' id='"+ data[i].id +"'><button class='btn btn-success btn-sm rounded-0' type='button' data-toggle='tooltip' data-placement='top' title='Editar'><i class='fa fa-edit'></i></button></li><li class='deleteemp list-inline-item' id='"+ data[i].id +"'><button class='btn btn-danger btn-sm rounded-0' type='button' data-toggle='tooltip' data-placement='top' title='Deletar'><i class='fa fa-trash'></i></button></li></ul>"

                $("#TabelaEmpresas > tbody").append("<tr> <td>  <a href='funcionarios.html?id=" + data[i].id + "&nome=" + encodeURIComponent(data[i].nome) + "'>" + data[i].nome + "</a></td> <td> " + data[i].uf + "</td> <td>" + html + "</td> </tr>");
            }

            
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) 
        {  

            if(XMLHttpRequest.status != 404)
            {
                LimparConfig();
                window.location.assign("login.html"); 
            }
     
        },
        contentType: "application/json",
        dataType: 'json'
      });  


     $(document).on('click', ".deleteemp", function (event) {
        event.preventDefault();

        var element =  event.currentTarget;        
        var IDEmpresa = event.currentTarget.id;        

        $.ajax({
            type: 'DELETE',
            url: 'https://' + host + ':'+ porta +'/empresa/deletar',
            async: false,
            headers: {Authorization: Final},
            data: JSON.stringify(IDEmpresa),
            success: function (data) 
            { 
                element.closest('tr').remove();
                $('#text_modal').html("Empresa removida com sucesso!</a>");
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
                    $('#text_modal').html("Não foi possível remover a empresa, tente novamente.");
                    $('#ModalError').modal('show');
                }
    
                //BtnReset($this);                
            },
            contentType: "application/json",
            dataType: 'json'
        });
    });

     
      $(document).on('click', ".editemp", function (event) {
        event.preventDefault();
        var IDEmpresa = event.currentTarget.id;          
        window.location.assign("empresaeditar.html?id=" + IDEmpresa);
    });
     
    document.getElementsByTagName("html")[0].style.visibility = "visible";
      //$('ul').on('click','li.deleteemp',function(){
     //       console.log("working");
    //    });

});