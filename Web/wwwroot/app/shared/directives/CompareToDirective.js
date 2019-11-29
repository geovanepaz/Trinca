;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .directive("compareTo", compareTo);

  function compareTo()
  {
    return {
      require: "ngModel",
      scope: {
        otherModelValue: "=compareTo"
      },
      link: function (scope, element, attributes, ngModel)
      {
        ngModel.$validators.compareTo = function (modelValue)
        {
          if (!!scope.otherModelValue)
          {
            return modelValue === scope.otherModelValue;
          }
        };
      }
    };
  }
})();
