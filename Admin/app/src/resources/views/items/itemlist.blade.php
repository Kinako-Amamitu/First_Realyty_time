@extends('layouts.app')
@section('title','アイテム一覧')
@section('h1','▼アイテム一覧▼')
@section('body')
    <table class="table table-bordered mx-auto p-2" style="width: 60%">
        <tr>
            <th>ID</th>
            <th>アイテム名</th>
            <th>種類</th>
            <th>効果</th>
            <th>説明</th>
        </tr>
        @foreach ($accounts as $itemdata)
            <tr>

                <td>{{$itemdata['id']}}</td>
                <td>{{$itemdata['item_name']}}</td>
                <td>{{$itemdata['lot']}}</td>
                <td>{{$itemdata['get']}}</td>
                <td>{{$itemdata['explanation']}}</td>

            </tr>
        @endforeach
    </table>
@endsection
