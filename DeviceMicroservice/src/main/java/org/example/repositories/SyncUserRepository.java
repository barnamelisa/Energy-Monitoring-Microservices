package org.example.repositories;

import org.example.entities.SyncUser;
import org.springframework.data.jpa.repository.JpaRepository;

public interface SyncUserRepository extends JpaRepository<SyncUser, String> {
}
