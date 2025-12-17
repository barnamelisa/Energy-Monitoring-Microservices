package org.example.services;

import org.example.entities.SyncUser;
import org.example.repositories.SyncUserRepository;
import org.springframework.stereotype.Service;

@Service
public class UserSyncMemoryService {

    private final SyncUserRepository repository;

    public UserSyncMemoryService(SyncUserRepository repository) {
        this.repository = repository;
    }

    public void addUser(String userId) {
        repository.save(new SyncUser(userId));
    }

    public boolean exists(String userId) {
        return repository.existsById(userId);
    }
}

