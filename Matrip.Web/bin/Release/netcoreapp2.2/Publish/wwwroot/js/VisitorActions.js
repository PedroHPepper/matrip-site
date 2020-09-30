$(document).ready(function () {
    MoverScrollOrdenacao();
    MudarOrdenacao();
    MudarImagePrincipalProduto();
    MudarQuantidadeProdutoCarrinho();

    MascaraCEP();
    MascaraCPF();
    AJAXBuscarCEP();
    



    PedidoBtnImprimir();
});
function PedidoBtnImprimir() {
    $(".btn-imprimir").click(function () {
        window.print();
    });
}


function AtualizarValores() {
    var produto = parseFloat($(".texto-produto").text().replace("R$", "").replace(".", "").replace(",", "."));
    var frete = parseFloat($(".card-footer input[name=frete]:checked").parent().find("label").text().replace("R$", "").replace(".", "").replace(",", "."));
    
    var total = produto + frete;
    
    $(".texto-frete").text(numberToReal(frete));
    $(".texto-total").text(numberToReal(total));
}
function LimparValores() {
    $(".texto-frete").text("-");
    $(".texto-total").text("-");
}
function EnderecoEntregaCardsLoading() {
    for (var i = 0; i < 3; i++) {
        $(".card-text")[i].innerHTML = "<br /><br /><img src='\\img\\loading.gif' />";
    }
}
function EnderecoEntregaCardsLimpar() {
    for (var i = 0; i < 3; i++) {
        $(".card-title")[i].innerHTML = "-";
        $(".card-text")[i].innerHTML = "-";
        $(".card-footer .text-muted")[i].innerHTML = "-";
    }
}

function AJAXBuscarCEP() {
    jQuery(".Zipcode").keyup(function () {
        OcultarMensagemDeErro();

        if (jQuery(this).val().length == 10) {

            var cep = RemoverMascara(jQuery(this).val());
            jQuery.ajax({
                type: "GET",
                url: "https://viacep.com.br/ws/" + cep + "/json/?callback=callback_name",
                dataType: "jsonp",
                error: function (data) {
                    ShowErrorMessage("Opps! tivemos um erro na busca pelo CEP! Parece que os servidores estão offline!");
                },
                success: function (data) {
                    if (data.erro == undefined) {
                        if (jQuery("#Country").length) {
                            jQuery("#Country").val("Brasil");
                        }
                        jQuery("#State").val(data.uf);
                        jQuery("#City").val(data.localidade);
                        jQuery("#Street").val(data.logradouro);
                        jQuery("#Neighborhood").val(data.bairro);
                        jQuery("#Complement").val(data.complemento);
                        if (jQuery("#StreetForTransference").length) {
                            jQuery("#StateForTransference").val(data.uf);
                            jQuery("#CityForTransference").val(data.localidade);
                            jQuery("#StreetForTransference").val(data.logradouro);
                            jQuery("#NeighborhoodForTransference").val(data.bairro);
                            jQuery("#ComplementForTransference").val(data.complemento);
                            jQuery("#CountryForTransference").val("Brasil");
                        }
                    } else {
                        ShowErrorMessage("O CEP informado não existe!");
                    }
                }
            });
        }
    });
}
function MascaraCEP() {
    jQuery(".Zipcode").mask("00.000-000");
}
function MascaraCPF() {
    jQuery(".CPF").mask("000.000.000-00");
}




function numberToReal(numero) {
    //console.info(numero);
    var numero = numero.toFixed(2).split('.');
    numero[0] = "R$ " + numero[0].split(/(?=(?:...)*$)/).join('.');
    return numero.join(',');
}
function MudarQuantidadeProdutoCarrinho() {
    $("#order .btn-primary").click(function () {
        if ($(this).hasClass("diminuir")) {
            OrquestradorDeAcoesProduto("diminuir", $(this));
        }
        if ($(this).hasClass("aumentar")) {
            OrquestradorDeAcoesProduto("aumentar", $(this));
        }
    });
}



function OrquestradorDeAcoesProduto(operacao, botao) {
    OcultarMensagemDeErro();
    /*
     * Carregamento dos valores
     */
    var pai = botao.parent().parent();

    var produtoId = pai.find(".inputProdutoId").val();
    var quantidadeEstoque = parseInt(pai.find(".inputQuantidadeEstoque").val());
    var valorUnitario = parseFloat(pai.find(".inputValorUnitario").val().replace(",", "."));

    var campoQuantidadeProdutoCarrinho = pai.find(".inputQuantidadeProdutoCarrinho");
    var quantidadeProdutoCarrinhoAntiga = parseInt(campoQuantidadeProdutoCarrinho.val());

    var campoValor = botao.parent().parent().parent().parent().parent().find(".price");

    var produto = new ProdutoQuantidadeEValor(produtoId, quantidadeEstoque, valorUnitario, quantidadeProdutoCarrinhoAntiga, 0, campoQuantidadeProdutoCarrinho, campoValor);

    /*
     * Chamada de Métodos
     */
    AlteracoesVisuaisProdutoCarrinho(produto, operacao);

    //TODO - Adicionar validações.

    //TODO - Atualizar o subtotal do produto
}
function AlteracoesVisuaisProdutoCarrinho(produto, operacao) {
    if (operacao == "aumentar") {
        /*if (produto.quantidadeProdutoCarrinhoAntiga == produto.quantidadeEstoque) {
            alert("Opps! Não possuimos estoque suficiente para a quantidade que você deseja comprar!");
        } else*/ {
            produto.quantidadeProdutoCarrinhoNova = produto.quantidadeProdutoCarrinhoAntiga + 1;

            AtualizarQuantidadeEValor(produto);

            AJAXComunicarAlteracaoQuantidadeProduto(produto);

        }
    } else if (operacao == "diminuir") {
        /*if (produto.quantidadeProdutoCarrinhoAntiga == 1) {
            alert("Opps! Caso não deseje este produto clique no botão Remover");
        } else */ {
            produto.quantidadeProdutoCarrinhoNova = produto.quantidadeProdutoCarrinhoAntiga - 1;

            AtualizarQuantidadeEValor(produto);

            AJAXComunicarAlteracaoQuantidadeProduto(produto);
        }
    }
}
function AJAXComunicarAlteracaoQuantidadeProduto(produto) {
    $.ajax({
        type: "GET",
        url: "/CarrinhoCompra/AlterarQuantidade?id=" + produto.produtoId + "&quantidade=" + produto.quantidadeProdutoCarrinhoNova,
        error: function (data) {
            ShowErrorMessage(data.responseJSON.mensagem);

            //Rollback
            produto.quantidadeProdutoCarrinhoNova = produto.quantidadeProdutoCarrinhoAntiga;
            AtualizarQuantidadeEValor(produto);
        },
        success: function () {
            AJAXCalcularFrete();
        }
    });
}
function ShowErrorMessage(mensagem) {
    $(".alert-danger").css("display", "block");
    $(".alert-danger").text(mensagem);
}
function OcultarMensagemDeErro() {
    $(".alert-danger").css("display", "none");
}

function AtualizarQuantidadeEValor(produto) {
    produto.campoQuantidadeProdutoCarrinho.val(produto.quantidadeProdutoCarrinhoNova);

    var resultado = produto.valorUnitario * produto.quantidadeProdutoCarrinhoNova;
    produto.campoValor.text(numberToReal(resultado));

    AtualizarSubtotal();
}
function AtualizarSubtotal() {
    var Subtotal = 0;

    var TagsComPrice = $(".price");

    TagsComPrice.each(function () {
        var ValorReais = parseFloat($(this).text().replace("R$", "").replace(".", "").replace(" ", "").replace(",", "."));

        Subtotal += ValorReais;
    })
    $(".subtotal").text(numberToReal(Subtotal));


}
function MudarImagePrincipalProduto() {
    $(".img-small-wrap img").click(function () {
        var Caminho = $(this).attr("src");
        $(".img-big-wrap img").attr("src", Caminho);
        $(".img-big-wrap a").attr("href", Caminho);
    });
}
function MoverScrollOrdenacao() {
    if (window.location.hash.length > 0) {
        var hash = window.location.hash;
        if (hash == "#posicao-produto") {
            window.scrollBy(0, 473);
        }
    }
}
function MudarOrdenacao() {
    $("#ordenacao").change(function () {
        var Pagina = 1;
        var Pesquisa = "";
        var Ordenacao = $(this).val();
        var Fragmento = "#posicao-produto";

        var QueryString = new URLSearchParams(window.location.search);
        if (QueryString.has("pagina")) {
            Pagina = QueryString.get("pagina");
        }
        if (QueryString.has("pesquisa")) {
            Pesquisa = QueryString.get("pesquisa");
        }
        if ($("#breadcrumb").length > 0) {
            Fragmento = "";
        }

        var URL = window.location.protocol + "//" + window.location.host + window.location.pathname;

        var URLComParametros = URL + "?pagina=" + Pagina + "&pesquisa=" + Pesquisa + "&ordenacao=" + Ordenacao + Fragmento;
        window.location.href = URLComParametros;

    });
}


/*
 * ------------------ Classes --------------------
 */
class ProdutoQuantidadeEValor {
    constructor(produtoId, quantidadeEstoque, valorUnitario, quantidadeProdutoCarrinhoAntiga, quantidadeProdutoCarrinhoNova, campoQuantidadeProdutoCarrinho, campoValor) {
        this.produtoId = produtoId;
        this.quantidadeEstoque = quantidadeEstoque;
        this.valorUnitario = valorUnitario;

        this.quantidadeProdutoCarrinhoAntiga = quantidadeProdutoCarrinhoAntiga;
        this.quantidadeProdutoCarrinhoNova = quantidadeProdutoCarrinhoNova;

        this.campoQuantidadeProdutoCarrinho = campoQuantidadeProdutoCarrinho;
        this.campoValor = campoValor;
    }
}

function RemoverMascara(valor) {
    return valor.replace(".", "").replace("-", "");
}