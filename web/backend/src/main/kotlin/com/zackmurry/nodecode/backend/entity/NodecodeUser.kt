package com.zackmurry.nodecode.backend.entity

import java.util.*
import javax.persistence.Entity
import javax.persistence.Id

@Entity
data class NodecodeUser(
    var username: String,
    @Id var id: UUID,
    var provider: String,
    var avatarUrl: String,
    var timeCreated: Long,
    var numPackages: Int
)
