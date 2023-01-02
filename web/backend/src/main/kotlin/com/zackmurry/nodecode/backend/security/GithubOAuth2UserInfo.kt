package com.zackmurry.nodecode.backend.security

class GithubOAuth2UserInfo(attributes: Map<String, Any>) : OAuth2UserInfo(attributes) {

    override fun getUsername(): String? {
        return attributes["login"] as String?
    }

    override fun getAvatarUrl(): String {
        return attributes["avatar_url"] as String
    }

}