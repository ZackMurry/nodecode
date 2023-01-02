package com.zackmurry.nodecode.backend.dao

import com.zackmurry.nodecode.backend.entity.PackageVersion
import org.springframework.data.jpa.repository.JpaRepository

interface PackageVersionDao : JpaRepository<PackageVersion, String> {

    fun findAllByPackageNameOrderByTimePublishedDesc(packageName: String): List<PackageVersion>

}