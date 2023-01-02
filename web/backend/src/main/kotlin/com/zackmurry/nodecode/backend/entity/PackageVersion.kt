package com.zackmurry.nodecode.backend.entity

import java.sql.Timestamp
import javax.persistence.Entity
import javax.persistence.Id

@Entity
data class PackageVersion(
    @Id val id: String,
    val packageName: String,
    val version: String,
    val timePublished: Long
)
