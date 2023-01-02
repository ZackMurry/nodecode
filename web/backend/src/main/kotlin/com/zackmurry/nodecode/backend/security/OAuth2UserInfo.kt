package com.zackmurry.nodecode.backend.security

abstract class OAuth2UserInfo(val attributes: Map<String, Any>) {

    abstract fun getUsername(): String?

    abstract fun getAvatarUrl(): String

}