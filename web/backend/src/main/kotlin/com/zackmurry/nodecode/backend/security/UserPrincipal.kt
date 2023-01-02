package com.zackmurry.nodecode.backend.security

import com.zackmurry.nodecode.backend.entity.NodecodeUser
import com.zackmurry.nodecode.backend.model.UserPrincipalResponse
import org.springframework.security.core.GrantedAuthority
import org.springframework.security.core.authority.SimpleGrantedAuthority
import org.springframework.security.core.userdetails.UserDetails
import org.springframework.security.oauth2.core.user.OAuth2User
import java.util.*


class UserPrincipal(
    private var username: String,
    private var id: UUID,
    private var authorities: MutableCollection<out GrantedAuthority>,
    private var attributes: MutableMap<String, Any>,
    private var avatarUrl: String,
    private var timeCreated: Long,
    private var numPackages: Int
) : OAuth2User, UserDetails {

    companion object {
        fun create(user: NodecodeUser): UserPrincipal {
            val grantedAuthorities = Collections.singletonList(SimpleGrantedAuthority("ROLE_USER"))
            return UserPrincipal(
                user.username,
                user.id,
                grantedAuthorities,
                HashMap(),
                user.avatarUrl,
                user.timeCreated,
                user.numPackages
            )
        }

        fun create(user: NodecodeUser, attributes: MutableMap<String, Any>): UserPrincipal {
            val userPrincipal = create(user)
            userPrincipal.attributes = attributes
            return userPrincipal
        }
    }

    override fun getUsername(): String {
        return this.username
    }

    override fun getPassword(): String? {
        return null
    }

    override fun isAccountNonExpired(): Boolean {
        return true
    }

    override fun isAccountNonLocked(): Boolean {
        return true
    }

    override fun isCredentialsNonExpired(): Boolean {
        return true
    }

    override fun isEnabled(): Boolean {
        return true
    }

    override fun getAttributes(): MutableMap<String, Any> {
        return this.attributes
    }

    override fun getAuthorities(): MutableCollection<out GrantedAuthority> {
        return this.authorities
    }

    override fun getName(): String {
        return this.username
    }

    fun getId(): UUID {
        return this.id
    }

    fun getAvatarUrl(): String {
        return avatarUrl
    }

    fun toResponse(): UserPrincipalResponse {
        return UserPrincipalResponse(
            username,
            id,
            authorities,
            attributes,
            isEnabled,
            isAccountNonLocked,
            isAccountNonExpired,
            isCredentialsNonExpired,
            name,
            avatarUrl,
            timeCreated,
            numPackages
        )
    }

}
