;
(function ()
{
  angular
    .module("boilerplate", ["ui.router", "ngMessages", "ng-sweet-alert", "ngMask", "ngTagsInput", "ui.toggle", "angular-autogrow", "moment-picker", "thatisuday.ng-spin", "timer", "disableAll", "credit-cards", "720kb.tooltips"])
    .config(config);

  config.$inject = ["$stateProvider", "$urlRouterProvider", "$httpProvider", "$qProvider", "ngSpinOpsProvider", "momentPickerProvider"];

  function config($stateProvider, $urlRouterProvider, $httpProvider, $qProvider, ngSpinOpsProvider, momentPickerProvider)
  {
    momentPickerProvider.options({ minutesStep: 30 });

    ngSpinOpsProvider.setOps({
      autoGlobal: true,
      spinner: 'worm',
      size: 'normal',
      color: '#333',
      position: 'center',
      blocking: false,
      delay: 0,
      extend: 1300
    });

    $urlRouterProvider.otherwise(function ($injector, $location)
    {
      var $state = $injector.get('$state');

      var url = $location.$location.$$absUrl;

      var directory = url.match(/([^\/]*)\/*$/)[1];

      if (url.indexOf('#!') === -1 && directory === 'dashboard')
      {
        $state.go('dashboard.painel');
      }
      else
      {
        return '/home';
      }
    });

    $qProvider.errorOnUnhandledRejections(false);

    $stateProvider
      .state("home",
        {
          url: "/home",
          templateUrl: "app/components/home/HomeView.html"
        })
      .state("home.novaconta",
        {
          url: "/nova-conta",
          templateUrl: "app/components/home/NovaContaView.html",
          controller: "NovaContaController",
          controllerAs: 'home'
        })
      .state("home.pin",
        {
          url: "/pin",
          templateUrl: "app/components/home/PinView.html",
          controller: "PinController",
          controllerAs: 'home'
        })
      .state("home.cadastro",
        {
          url: "/cadastro",
          templateUrl: "app/components/home/CadastroView.html",
          controller: "CadastroController",
          controllerAs: 'home'
        })
      .state("home.planos",
        {
          url: "/planos",
          templateUrl: "app/components/home/PlanosView.html",
          controller: "PlanosController",
          controllerAs: 'home'
        })
      .state("home.perfil",
        {
          url: "/perfil",
          templateUrl: "app/components/home/PerfilView.html",
          controller: "PerfilController",
          controllerAs: 'home'
        })
      .state("home.login",
        {
          url: "/login",
          templateUrl: "app/components/home/LoginView.html",
          controller: "LoginController",
          controllerAs: 'home'
        })
      .state('dashboard',
        {
          url: "/"
        })
      .state('dashboard.painel',
        {
          url: "painel",
          templateUrl: "/app/components/dashboard/PainelView.html",
          controller: "PainelController"
        })
      .state('dashboard.minhaconta',
        {
          url: "minha-conta",
          templateUrl: "/app/components/dashboard/MinhaContaView.html",
          controller: "MinhaContaController",
          controllerAs: 'ctrl'
        })
      .state('dashboard.perfis',
        {
          url: "perfis",
          templateUrl: "/app/components/dashboard/PerfisView.html",
          controller: "PerfisController",
          controllerAs: 'ctrl'
        })
      .state('dashboard.locais',
        {
          url: "locais",
          templateUrl: "/app/components/dashboard/LocaisView.html",
          controller: "LocaisController",
          controllerAs: 'ctrl'
        })
      .state('dashboard.hashes',
        {
          url: "hashes",
          templateUrl: "/app/components/dashboard/HashtagView.html",
          controller: "HashtagController",
          controllerAs: 'ctrl'
        })
      .state('dashboard.agendamento',
        {
          url: "agendamento/:id",
          templateUrl: "/app/components/dashboard/AgendamentoView.html",
          controller: "AgendamentoController",
          controllerAs: 'ctrl',
          params: { id: { squash: true, value: null } }
        })
      .state('dashboard.postagens',
        {
          url: "postagens",
          templateUrl: "/app/components/dashboard/PostagensView.html",
          controller: "PostagensController",
          controllerAs: 'ctrl'
        })
      .state('dashboard.chatbot',
        {
          url: "chatbot",
          templateUrl: "/app/components/dashboard/ChatbotView.html",
          controller: "ChatbotController",
          controllerAs: 'ctrl'
        })
      .state('dashboard.gerenciarperfis',
        {
          url: "gerenciar-perfis",
          templateUrl: "/app/components/dashboard/GerenciarPerfisView.html",
          controller: "GerenciarPerfisController",
          controllerAs: 'ctrl'
        })
      .state('dashboard.comentarios',
        {
          url: "comentarios",
          templateUrl: "/app/components/dashboard/ComentariosView.html",
          controller: "ComentariosController",
          controllerAs: 'ctrl'
        })
      .state('dashboard.configuracoes',
        {
          url: "configuracoes",
          templateUrl: "/app/components/dashboard/ConfiguracoesView.html",
          controller: "ConfiguracoesController",
          controllerAs: 'ctrl'
        });

    $httpProvider.interceptors.push('authInterceptor');

    $httpProvider.defaults.headers.common['Cache-Control'] = 'no-cache';
    $httpProvider.defaults.cache = false;
  }

  angular
    .module("boilerplate")
    .factory("authInterceptor", authInterceptor);

  authInterceptor.$inject = ["$rootScope", "$q", "LocalStorage", "$location", "SweetAlert", "$document"];

  function authInterceptor($rootScope, $q, LocalStorage, $location, SweetAlert, $document)
  {
    return {

      // Qualquer request
      request: function (config)
      {
        return config;
      },

      response: function (response)
      {
        return response;
      },

      // Erros
      responseError: function (response)
      {
        // BAD REQUEST: Dados inválidos de cadastro, atualização etc.
        if (response.status === 400) 
        {
          var mensagemHtml = "<ul class='text-left'>";

          angular.forEach(response.data.errors, function (value, key)
          {
            angular.forEach(value, function (a)
            {
              mensagemHtml += "<li>" + a + "</li>";
            });

          });

          mensagemHtml += "</ul>";

          SweetAlert.error(mensagemHtml, { title: "Ops! Alguns problemas surgiram", html: true });

          return $q.reject(response);
        }

        // INTERNAL SERVER ERROR: Alguma exceção interna
        else if (response.status === 500)
        {
          SweetAlert.error(`(${response.data.Error}) - ${response.data.Message}`, { title: "Ops!" });

          return $q.reject(response);
        }

        // UNAUTHORIZED: O servidor não autorizou o consumo do recurso
        else if (response.status === 401)
        {
          SweetAlert.alert(`${response.data.Message}`, { title: "Ops!" });

          return $q.reject(response);
        }

        // Api não está respondendo
        else if (response.status === -1 && response.data === null)
        {
          SweetAlert.error("Não foi possível conectar-se ao servidor", { title: "Ops!" });

          return $q.reject(response);
        }

        else
        {
          return $q.reject(response);
        }
      }
    };
  }

  /**
   * Run block
   */
  angular
    .module("boilerplate")
    .run(run);

  run.$inject = ["$rootScope", "$location"];

  function run($rootScope, $location)
  {

    // put here everything that you need to run on page load

  }

})();
