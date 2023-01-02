package com.zackmurry.nodecode.backend.model

import java.util.*


data class UserResponse(
    var id: UUID,
    var username: String,
    var avatarUrl: String,
    var timeCreated: Long,
    var numPackages: Int
)
