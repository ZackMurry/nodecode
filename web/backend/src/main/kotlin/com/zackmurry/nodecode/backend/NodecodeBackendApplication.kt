package com.zackmurry.nodecode.backend

import org.springframework.boot.autoconfigure.SpringBootApplication
import org.springframework.boot.runApplication

@SpringBootApplication
class NodecodeBackendApplication

fun main(args: Array<String>) {
    runApplication<NodecodeBackendApplication>(*args)
}
