;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .factory("CadastroService", ["QueryService", cadastroService]);

  function cadastroService(QueryService)
  {
    return {
      ramosAtividades: ramosAtividades,
      planos: planos,
      atualizarCadastro: atualizarCadastro,
      atualizarSenha: atualizarSenha
    };

    //////////////// functions

    // Carrega os planos
    function planos(exibirTrial)
    {
      return QueryService
        .query("GET", "cadastros/planos", { exibirTrial: exibirTrial })
        .then(function (d)
        {
          return d.data;
        });
    }

    function ramosAtividades()
    {
      return QueryService
        .query("GET", "cadastros/ramos-atividades")
        .then(function (d)
        {
          return d.data;
        });
    }

    function atualizarCadastro(dados)
    {
      return QueryService
        .query("PATCH", "cadastros", null, dados)
        .then(function (d)
        {
          return d.data;
        });
    }

    function atualizarSenha(dados)
    {
      return QueryService
        .query("PATCH", "cadastros/senhas", null, dados)
        .then(function (d)
        {
          return d.data;
        });
    }

  }

})();
