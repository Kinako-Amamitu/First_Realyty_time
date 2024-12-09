<?php

namespace Database\Seeders;

use App\Models\User;
use Illuminate\Database\Console\Seeds\WithoutModelEvents;
use Illuminate\Database\Seeder;
use Illuminate\Support\Facades\Hash;

class UserTableSeeder extends Seeder
{
    /**
     * Run the database seeds.
     */
    public function run(): void
    {
        User::create([
            'name' => "kidakosuke",
            'token' => Hash::make('122084')
        ]);
        User::create([
            'name' => "kosukekida",
            'token' => Hash::make('679364')
        ]);
        User::create([
            'name' => "ikeda",
            'token' => Hash::make('672579')
        ]);
    }
}
