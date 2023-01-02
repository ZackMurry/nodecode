package com.zackmurry.nodecode.backend.entity

import java.sql.Timestamp
import java.util.*
import javax.persistence.Entity
import javax.persistence.Id

@Entity
data class Package(
    @Id var name: String,
    var authorId: UUID,
    var description: String,
    var lastUpdated: Long,
    var documentationUrl: String?,
    var repositoryUrl: String?,
    var downloads: Int,
    var latestVersion: String?
)
