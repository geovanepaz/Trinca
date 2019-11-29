;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .controller("PainelController", PainelController);

  PainelController.$inject = ["$scope", "$state"];

  function PainelController($scope, $state)
  {
    // 'controller as'
    const self = this;
  }
})();
