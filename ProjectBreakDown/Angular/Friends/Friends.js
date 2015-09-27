'use strict';

angular
    .module('PBD.friends', ['ngRoute'])
    .config(config)
    .controller('friendsCtrl', friendsCtrl);

config.$inject = ['$routeProvider'];
friendsCtrl.$inject = ['$window', 'AuthService', 'FriendService'];

/////////////////////////////////
//CONFIG
////////////////////////////////

function config($routeProvider) {
    $routeProvider.when('/friends', {
        templateUrl: 'Angular/friends/friends.html',
        controller: 'friendsCtrl',
        controllerAs: 'vm',
        resolve: {
            requireAuth: requireAuth //see if there is a way to add this resolve to every page except login page in app config
        }
    });

    function requireAuth($location, AuthService) {
        if(AuthService.Token() === null)
            $location.path('/login');
    }
}

function friendsCtrl($window, AuthService, FriendService) {
    var vm = this;
    vm.friends = [];
    FriendService.GetFriends().then(updateFriends);

    function updateFriends(response) {
        vm.friends = response.data;
    }
};

