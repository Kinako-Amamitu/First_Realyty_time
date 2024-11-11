<?php

use Illuminate\Database\Migrations\Migration;
use Illuminate\Database\Schema\Blueprint;
use Illuminate\Support\Facades\Schema;

return new class extends Migration {
    public function up(): void
    {
        Schema::create('frends', function (Blueprint $table) {
            $table->id();
            $table->integer('user_id');
            $table->integer('follow_user_id');
            $table->boolean('follow_check');
            $table->timestamps();
        });
    }

    public function down(): void
    {
        Schema::dropIfExists('frends');
    }
};
