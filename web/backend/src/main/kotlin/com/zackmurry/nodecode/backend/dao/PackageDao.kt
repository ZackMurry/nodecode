package com.zackmurry.nodecode.backend.dao

import com.zackmurry.nodecode.backend.entity.Package
import com.zackmurry.nodecode.backend.model.PackagePreviewResponse
import org.springframework.data.jpa.repository.JpaRepository
import org.springframework.data.jpa.repository.Modifying
import org.springframework.data.jpa.repository.Query
import org.springframework.transaction.annotation.Transactional
import java.util.*

@Transactional
interface PackageDao : JpaRepository<Package, String> {

    fun findAllByAuthorIdOrderByLastUpdatedDesc(authorId: UUID): List<Package>

    fun findTop5ByOrderByLastUpdated(): List<Package>

    fun findTop5ByOrderByDownloads(): List<Package>

    @Modifying
    @Query("UPDATE package SET last_updated = :time WHERE name = :id", nativeQuery = true)
    fun updateLastUpdatedById(id: String, time: Long = System.currentTimeMillis())

    @Modifying
    @Query("UPDATE package SET latest_version = :latestVersion, last_updated = :time WHERE name = :id", nativeQuery = true)
    fun updateLatestVersionById(id: String, latestVersion: String, time: Long = System.currentTimeMillis())

    @Modifying
    @Query("UPDATE package SET downloads = downloads + 1 WHERE name = :id", nativeQuery = true)
    fun incrementDownloadsById(id: String): Any

    @Modifying
    @Query("UPDATE package SET description = :description, documentation_url = :documentationUrl, repository_url = :repositoryUrl WHERE name = :id", nativeQuery = true)
    fun updatePackageById(id: String, description: String, documentationUrl: String?, repositoryUrl: String?)

    // todo limit and next page (not really necessary for demo though)
    @Query("SELECT * FROM package WHERE ts @@ to_tsquery(:query) ORDER BY ts_rank_cd(ts, to_tsquery(:query))", nativeQuery = true)
    fun searchPackages(query: String): List<Package>

}
