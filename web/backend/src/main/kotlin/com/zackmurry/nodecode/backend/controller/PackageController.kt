package com.zackmurry.nodecode.backend.controller

import com.zackmurry.nodecode.backend.exception.BadRequestException
import com.zackmurry.nodecode.backend.model.PackageCreateRequest
import com.zackmurry.nodecode.backend.model.PackagePreviewResponse
import com.zackmurry.nodecode.backend.model.PackageUpdateRequest
import com.zackmurry.nodecode.backend.model.TrendingPackagesResponse
import com.zackmurry.nodecode.backend.service.PackageService
import com.zackmurry.nodecode.backend.service.PackageVersionService
import org.springframework.core.io.ByteArrayResource
import org.springframework.http.ResponseEntity
import org.springframework.web.bind.annotation.*
import org.springframework.web.multipart.MultipartFile

@RequestMapping("/api/v1/packages")
@RestController
class PackageController(val packageService: PackageService, val packageVersionService: PackageVersionService) {

    @GetMapping("/name/{name}")
    fun getPackageDetails(@PathVariable name: String) = packageService.getPackageDetails(name)

    @PostMapping
    fun createPackage(@RequestBody request: PackageCreateRequest) = packageService.createPackage(request)

    @PostMapping("/name/{name}/versions/{version}")
    fun addVersionToPackage(
        @PathVariable name: String,
        @PathVariable version: String,
        @RequestParam("file") file: MultipartFile
    ) {
        if (!packageService.existsByName(name)) {
            throw BadRequestException()
        }
        packageVersionService.addVersionToPackage(name, version, file)
        packageService.updateLatestVersion(name, version)
    }

    @GetMapping("/name/{name}/versions/{version}/raw")
    fun getRawPackageFile(@PathVariable name: String, @PathVariable version: String): ResponseEntity<ByteArrayResource> {
        val response = packageVersionService.getRawPackageFile(name, version)
        packageService.incrementPackageDownloads(name) // Only increment downloads if no error
        return response
    }

    @PutMapping("/name/{name}")
    fun updatePackageDetails(@PathVariable name: String, @RequestBody request: PackageUpdateRequest) {
        packageService.updatePackageDetails(name, request)
    }

    @GetMapping("/trending")
    fun getTrendingPackages(): TrendingPackagesResponse {
        val popular = packageService.getPopularPackages()
        val latest = packageService.getNewPackages()
        return TrendingPackagesResponse(popular, latest)
    }

    @GetMapping("/search")
    fun searchPackages(@RequestParam q: String): List<PackagePreviewResponse> {
        println(q)
        return packageService.searchPackages(q)
    }

}
