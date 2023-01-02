package com.zackmurry.nodecode.backend.model

import org.springframework.security.core.GrantedAuthority
import java.util.*


class UserPrincipalResponse(
    val username: String,
    uuid: UUID,
    authoritiesMap: MutableCollection<out GrantedAuthority>,
    attributesMap: MutableMap<String, Any>,
    val enabled: Boolean,
    val accountNonLocked: Boolean,
    val accountNonExpired: Boolean,
    val credentialsNonExpired: Boolean,
    val name: String,
    val avatarUrl: String,
    var timeCreated: Long,
    var numPackages: Int
) {
    val id = uuid.toString()
}
