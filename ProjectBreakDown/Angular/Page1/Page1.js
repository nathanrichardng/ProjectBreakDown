'use strict';

angular.module('PBD.page1', ['ngRoute'])

.config(['$routeProvider', function ($routeProvider) {
    $routeProvider.when('/page1', {
        templateUrl: 'Angular/Page1/Page1.html',
        controller: 'Page1Ctrl',
        controllerAs: 'vm'
    });
}])

.controller('Page1Ctrl', [function () {
    var vm = this;
    vm.message = "Hello ASP.Net!";
}]);