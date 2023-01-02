package com.zackmurry.nodecode.backend.security

import com.zackmurry.nodecode.backend.exception.OAuth2AuthenticationProcessingException

object OAuth2UserInfoFactory {

    fun getOAuth2UserInfo(registrationId: String, attributes: Map<String, Any>): OAuth2UserInfo {
        if (registrationId.uppercase() == AuthProvider.GITHUB.toString()) {
            return GithubOAuth2UserInfo(attributes)
        } else {
            throw OAuth2AuthenticationProcessingException("Login with $registrationId is not supported")
        }
    }

}