'use strict';

//inject service into app
angular
    .module('PBD')
    .factory('FriendService', FriendService);

FriendService.$inject = ['$http', '$httpParamSerializer'];

function FriendService($http, $httpParamSerializer) {

    return {
        GetFriends: GetFriends
        //more method declarations go here
    }

    function GetFriends() {

        return $http.get('/Api/Friends');

    }


    //More methods go here. need to add "add friend request method"

}

