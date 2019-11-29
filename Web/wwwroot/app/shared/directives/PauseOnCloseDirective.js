;
(function ()
{
  "use strict";

  angular
    .module("boilerplate")
    .directive("pauseOnClose", pauseOnClose);

  function pauseOnClose()
  {
    return {
      restrict: 'A',
      link: function (scope, element, attrs)
      {
        element.on('hidden.bs.modal',
          function (e)
          {
            // Find elements by video tag
            var nodesArray = [].slice.call(document.querySelectorAll("video"));
            // Loop through each video element 
            angular.forEach(nodesArray,
              function (obj)
              {
                // Apply pause to the object
                obj.pause();
              });
          });
      }
    };
  }
})();
