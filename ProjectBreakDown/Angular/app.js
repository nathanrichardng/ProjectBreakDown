'use strict';

// Declare app level module which depends on views, and components
angular
	.module('PBD', [
	  'ngRoute',
      'PBD.page1',
      'PBD.friends',
      'PBD.friendRequests',
      'PBD.login'
	])
	.config(config)
	.controller('appCtrl', appCtrl);

config.$inject = ['$routeProvider', '$httpProvider'];

function config($routeProvider, $httpProvider) {
    $routeProvider.otherwise({ redirectTo: '/home' });
    $httpProvider.defaults.headers.post['Accept'] = 'application/x-www-form-urlencoded';
    $httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded';
}

function appCtrl() {
    var vm = this;
    vm.message = "Hello ASP.Net!"
}