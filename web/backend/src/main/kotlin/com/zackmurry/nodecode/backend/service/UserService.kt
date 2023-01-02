package com.zackmurry.nodecode.backend.service

import com.zackmurry.nodecode.backend.dao.UserDao
import com.zackmurry.nodecode.backend.entity.NodecodeUser
import com.zackmurry.nodecode.backend.exception.NotFoundException
import com.zackmurry.nodecode.backend.model.UserResponse
import com.zackmurry.nodecode.backend.security.UserPrincipal
import org.springframework.data.repository.findByIdOrNull
import org.springframework.security.core.context.SecurityContextHolder
import org.springframework.security.core.userdetails.UserDetails
import org.springframework.security.core.userdetails.UserDetailsService
import org.springframework.security.core.userdetails.UsernameNotFoundException
import org.springframework.stereotype.Service
import java.util.*

@Service
class UserService(private val userDao: UserDao) : UserDetailsService {

    companion object {
        fun getUserId(): UUID {
            return (SecurityContextHolder.getContext().authentication.principal as UserPrincipal).getId()
        }
    }

    override fun loadUserByUsername(username: String?): UserDetails {
        if (username == null) {
            throw UsernameNotFoundException(null)
        }
        val user = userDao.findByUsername(username).orElseThrow { throw NotFoundException() }
        return UserPrincipal.create(user)
    }

    fun createUser(user: NodecodeUser) {
        userDao.save(user)
    }

    fun accountExists(username: String): Boolean {
        return userDao.existsByUsername(username)
    }

    fun getUserByUsername(username: String): NodecodeUser? {
        return userDao.findByUsername(username).orElse(null)
    }

    fun deleteByUsername(username: String) {
        userDao.deleteByUsername(username)
    }

    fun getUserResponse(id: UUID): UserResponse {
        val user = userDao.findByIdOrNull(id) ?: throw NotFoundException()
        return UserResponse(user.id, user.username, user.avatarUrl, user.timeCreated, user.numPackages)
    }

    fun incrementNumPackages(userId: UUID) = userDao.incrementNumPackagesByUserId(userId)

}
