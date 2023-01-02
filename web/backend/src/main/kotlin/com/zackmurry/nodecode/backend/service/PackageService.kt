package com.zackmurry.nodecode.backend.service

import com.zackmurry.nodecode.backend.dao.PackageDao
import com.zackmurry.nodecode.backend.entity.Package
import com.zackmurry.nodecode.backend.exception.BadRequestException
import com.zackmurry.nodecode.backend.exception.ConflictException
import com.zackmurry.nodecode.backend.exception.ForbiddenException
import com.zackmurry.nodecode.backend.exception.NotFoundException
import com.zackmurry.nodecode.backend.model.PackageCreateRequest
import com.zackmurry.nodecode.backend.model.PackagePreviewResponse
import com.zackmurry.nodecode.backend.model.PackageResponse
import com.zackmurry.nodecode.backend.model.PackageUpdateRequest
import com.zackmurry.nodecode.backend.security.UserPrincipal
import org.springframework.data.repository.findByIdOrNull
import org.springframework.security.core.context.SecurityContextHolder
import org.springframework.stereotype.Service
import java.util.*
import kotlin.streams.toList

private val PACKAGE_NAME_REGEX = Regex("[a-z|0-9]+(-[a-z|0-9]+)*")

@Service
class PackageService(
    private val packageDao: PackageDao,
    private val userService: UserService,
    private val packageVersionService: PackageVersionService
) {

    fun getPackageDetails(name: String): PackageResponse {
        val pkg = packageDao.findByIdOrNull(name) ?: throw NotFoundException()
        val author = userService.getUserResponse(pkg.authorId)
        val pkgVersions = packageVersionService.getPackageVersionsByPackage(name)
        var isOwnedByUser = false
        val principal = SecurityContextHolder.getContext().authentication.principal
        if (principal.toString() != "anonymousUser" && (principal as UserPrincipal).getId() == author.id) {
            isOwnedByUser = true
        }
        return PackageResponse.from(pkg, author, pkgVersions, isOwnedByUser)
    }

    fun createPackage(request: PackageCreateRequest) {
        if (request.name.length > 32 || request.name.length < 3 || request.description.length > 200 || (request.documentationUrl != null && request.documentationUrl!!.length > 5000) || (request.repositoryUrl != null && request.repositoryUrl!!.length > 5000)) {
            throw BadRequestException()
        }
        if (!PACKAGE_NAME_REGEX.matches(request.name)) {
            throw BadRequestException()
        }
        if (packageDao.findByIdOrNull(request.name) != null) {
            throw ConflictException()
        }
        val userId = (SecurityContextHolder.getContext().authentication.principal as UserPrincipal).getId()
        packageDao.save(
            Package(
                request.name,
                userId,
                request.description,
                System.currentTimeMillis(),
                request.documentationUrl,
                request.repositoryUrl,
                0,
                null
            )
        )
        userService.incrementNumPackages(userId)
    }

    fun existsByName(name: String) = packageDao.existsById(name)

    fun updateLatestVersion(name: String, version: String) {
        packageDao.updateLatestVersionById(name, version)
    }

    /**
     * Sorted by most recently updated
     */
    fun getPackagesByUser(name: String): List<PackagePreviewResponse> {
        val userId = (SecurityContextHolder.getContext().authentication.principal as UserPrincipal).getId()
        return packageDao.findAllByAuthorIdOrderByLastUpdatedDesc(userId).map { PackagePreviewResponse.from(it) }
    }

    fun incrementPackageDownloads(name: String) = packageDao.incrementDownloadsById(name)

    fun updatePackageDetails(name: String, request: PackageUpdateRequest) {
        val pkg = packageDao.findById(name).orElseThrow { NotFoundException() }
        val userId = (SecurityContextHolder.getContext().authentication.principal as UserPrincipal).getId()
        if (pkg.authorId != userId) {
            throw ForbiddenException()
        }
        packageDao.updatePackageById(name, request.description, request.documentationUrl, request.repositoryUrl)
    }

    fun getPopularPackages(): List<PackagePreviewResponse> {
        return packageDao.findTop5ByOrderByDownloads().map { PackagePreviewResponse.from(it) }
    }

    fun getNewPackages(): List<PackagePreviewResponse> {
        return packageDao.findTop5ByOrderByLastUpdated().map { PackagePreviewResponse.from(it)}
    }

    fun searchPackages(q: String): List<PackagePreviewResponse> {
        return packageDao.searchPackages(q).stream().map { PackagePreviewResponse.from(it) }.toList()
    }

}