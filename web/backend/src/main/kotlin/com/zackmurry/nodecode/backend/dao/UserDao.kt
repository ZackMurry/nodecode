package com.zackmurry.nodecode.backend.dao

import com.zackmurry.nodecode.backend.entity.NodecodeUser
import org.springframework.data.jpa.repository.JpaRepository
import org.springframework.data.jpa.repository.Modifying
import org.springframework.data.jpa.repository.Query
import org.springframework.transaction.annotation.Transactional
import java.util.*

@Transactional
interface UserDao : JpaRepository<NodecodeUser, UUID> {

    fun findByUsername(username: String): Optional<NodecodeUser>

    fun existsByUsername(username: String): Boolean

    fun deleteByUsername(username: String)

    @Modifying
    @Query("UPDATE nodecode_user SET num_packages = num_packages + 1 WHERE id = :userId", nativeQuery = true)
    fun incrementNumPackagesByUserId(userId: UUID)

}
