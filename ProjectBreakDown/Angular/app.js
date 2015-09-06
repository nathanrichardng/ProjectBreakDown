'use strict';

// Declare app level module which depends on views, and components
angular
	.module('PBD', [
	  'ngRoute',
      'PBD.page1'
	])
	.config(config)
	.controller('appCtrl', appCtrl);

config.$inject = ['$routeProvider'];

function config($routeProvider) {
    $routeProvider.otherwise({ redirectTo: '/home' });
}

function appCtrl() {
    var vm = this;
    vm.message = "Hello ASP.Net!"
}